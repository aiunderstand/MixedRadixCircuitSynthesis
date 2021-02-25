// classes example
#include <iostream>
#include <fstream>
#include <vector>
#include <algorithm>
#include <string>
using namespace std;

class IO {
    int nr;
    vector<string> connections;
public:
    int getNr() { return nr; }
    void setNr(int i) { nr = i; }
};

class Connection {
    //hmmm...
};

class Subcircuit {
    string connections[4];    //interface connections i0, i1, i2, out
    //bool ap, an, bp, bn, cp, cn; //subcircuit interface
    bool inverters[9];
    string index;
    int arity;
public:
    void setConnection(int, string);
    void setInverters(int, bool);
    void setIndex(string indx) { index = indx; };
    void setArity(int arty) { arity = arty; }
    string getConnection(int i) { return connections[i]; }
    bool getInverter(int i) { return inverters[i]; }
    string getIndex() { return index; }
    int getArity() { return arity; }
};

void Subcircuit::setConnection(int i, string id) {
    connections[i] = id;     //i is the nr of the input, id is the id of the connection
}

void::Subcircuit::setInverters(int i, bool x) {
    inverters[i] = x;
}




///////////////////////////////// DATA
vector<vector<string>> connections = {
    {"in0", "in1", "", "out_ckt0"}  ,       //20K
    {"out_ckt0", "in2", "", "out0"}  ,      //20K
    {"in0", "in1", "", "out_ckt2"},         //K00
    {"out_ckt0", "in2", "", "out_ckt3"},    //K00
    {"out_ckt2", "out_ckt3", "", "out2"},   //ZKK
};

vector<vector<bool>> inv = {
    {true,true,false,true,true,false,false,false,false},
    {true,true,false,true,true,false,false,false,false},
    {false,true,false,false,true,false,false,false,false},
    {false,true,false,false,true,false,false,false,false},
    {false,true,false,false,true,false,false,false,false}
};


vector<string> index = {
    "20K", "20K", "K00", "K00", "ZKK"
};

vector<int> arity = { 2,2,2,2,2 };

vector<string> parsedInputNames;
vector<string> parsedOutputNames;
vector<string> parsedPositions;
vector<string> savedCircuitNames;
vector<int> connectionIndices;

///////////////////////////////// DATA
vector<Subcircuit> netlists;

void enterData(int compCount) {
    for (int i = 0; i < compCount; i++) {

        netlists[i].setIndex(index[i]);
        netlists[i].setArity(arity[i]);
        // entering connection data
        for (int j = 0; j < connections[i].size(); j++) {
            netlists[i].setConnection(j, connections[i][j]);
        }

        // entering subcircuit interface data
        for (int j = 0; j < 9; j++) {
            netlists[i].setInverters(j, inv[i][j]);
        }
    }
}


void printData(int compCount) {
    for (int i = 0; i < compCount; i++) {


        cout << "\nobject nr " << i << endl;
        cout << "subcircuit interface: \n";

        for (int j = 0; j < 9; j++) {
            cout << netlists[i].getInverter(j) << " ";
        }
        cout << endl;
        for (int j = 0; j < connections[i].size(); j++) {
            cout << "connection " << j << " " << netlists[i].getConnection(j) << endl;
        }

    }

}


vector<vector<string>> ParseConnectionIntoVectorStructure(char** a, int rows)
{
    vector<vector<string>>connVector;
    for (size_t i = 0; i < rows; i++)
    {
        vector<string> tempVector;

        //create a row of vectors, with fixed length 4 (due to arity 3, arity 2 will not use third spot)
        for (size_t j = 0; j < connectionIndices[i]; j++)
        {
            tempVector.push_back(a[i* connectionIndices[i] + j]);
        }

        connVector.push_back(tempVector);
    }

    return connVector;
}

vector<vector<bool>> ParseInverterIntoBoolStructure(int* a, int rows)
{
    vector<vector<bool>>invVector;
    for (size_t i = 0; i < rows; i++)
    {
        vector<bool> tempVector;
        
        //create a row of vectors, with fixed length 9 (due to ternary, binary will avoid middle value)
        for (size_t j = 0; j < 9; j++)
        {
            if (a[i*9 + j] == 0)
                tempVector.push_back(false);
            else
                tempVector.push_back(true);
        }

        invVector.push_back(tempVector);
    }

    return invVector;
}

vector<int> ParseIntArrayIntoIntVector(int* a, int length)
{
    vector<int> v;
    for (size_t i = 0; i < length; i++)
    {
        v.push_back(a[i]);
    }
    
    return v;
}

vector<string> ParseCharArrayIntoStringVector(char** a, int length)
{
    vector<string> v;
    for (size_t i = 0; i < length; i++)
    {
        v.push_back(a[i]);
    }

    return v;
}

extern "C" __declspec(dllexport) int CreateCircuit(
    char* filePath, 
    char* fileName, 
    int inputs,
    char** inputNames, 
    int outputs,
    char** outputNames, 
    int ttIndicesCount, 
    char** ttIndices,
    int arityCount,
    int* arityArray,
    int connecionCount,
    char** connectionArray,
    int invCount,
    int* invArray,
    int positionCount,
    char** positionArray,
    int savedCircuitCount, 
    char** savedCircuitNamesArray,
    int connectionIndexCount,
    int* connectionIndexArray) {

    //STEP 1: assign parameters with some conversion due to c# to c++ (we can refactor this as some of the fucntions use the same code)
    connectionIndices = ParseIntArrayIntoIntVector(connectionIndexArray, connectionIndexCount);
    connections = ParseConnectionIntoVectorStructure(connectionArray, connectionIndexCount);
    inv = ParseInverterIntoBoolStructure(invArray, ttIndicesCount);
    index = ParseCharArrayIntoStringVector(ttIndices, ttIndicesCount);
    arity = ParseIntArrayIntoIntVector(arityArray, ttIndicesCount);
    parsedInputNames = ParseCharArrayIntoStringVector(inputNames, inputs);
    parsedOutputNames = ParseCharArrayIntoStringVector(outputNames, outputs);
    parsedPositions = ParseCharArrayIntoStringVector(positionArray, positionCount); //2 coordinates -- x,y -- per component
    savedCircuitNames = ParseCharArrayIntoStringVector(savedCircuitNamesArray, savedCircuitCount);
   
    //STEP 2: INIT
    IO input;
    input.setNr(inputs);
    IO output;
    output.setNr(outputs);

  
    netlists.clear();
    netlists.resize(ttIndicesCount);
        
    enterData(ttIndicesCount);
    //printData(ttIndicesCount);

    //STEP 3: generate file
    ofstream myfile;
    string path = filePath;
    path += fileName;
    path += ".sp";
    myfile.open(path);

    myfile << "*** STATS" << endl;
    myfile << "*** @tcount 0" << endl;
    myfile << "*** @gcount 0" << endl;
    myfile << "*** @ugcount 0" << endl;
    myfile << "*** @abslvl 0" << endl;

    myfile << "\n*** SEMANTIC INTERFACE" << endl;
    myfile << "*** @inputs 0"  << endl;

    string inputlbls = "";
    for (int i = 0; i < input.getNr(); i++) {
        inputlbls += parsedInputNames[i] + " ";
    }
    myfile << "*** @inputlbl " + inputlbls << endl;

    myfile << "*** @outputs 0" << endl;
    
    string outputlbls = "";
    for (int i = 0; i < output.getNr(); i++) {
        outputlbls += parsedOutputNames[i] + " ";
    }
    myfile << "*** @outputlbl " + outputlbls << endl;

    myfile << "\n.subckt " << fileName;
    for (int i = 0; i < input.getNr(); i++) {
        myfile << " " << parsedInputNames[i];
    }
    for (int i = 0; i < output.getNr(); i++) {
        myfile << " " << parsedOutputNames[i];
    }
    myfile << " vdd\n";
    myfile << ".lib 'CNFET.lib' CNFET \n";

    vector<string> includedIndex;

    for (int i = 0; i < netlists.size(); i++) {
        if (find(includedIndex.begin(), includedIndex.end(), netlists[i].getIndex()) == includedIndex.end()) {

            myfile << ".include \"f_" << netlists[i].getIndex() << ".sp\"" << endl;
            includedIndex.push_back(netlists[i].getIndex());
        }

    }

    for (int i = 0; i < savedCircuitCount; i++) {
        if (find(includedIndex.begin(), includedIndex.end(), savedCircuitNames[i]) == includedIndex.end()) {

            myfile << ".include \"" << savedCircuitNames[i] << ".sp\"" << endl;
            includedIndex.push_back(savedCircuitNames[i]);
        }
    }

    myfile << ".include \"nti.sp\" \n.include \"pti.sp\"\n\n";

    vector<string> usedInverters;
    int inverterCount = 0;
    for (int i = 0; i < netlists.size(); i++) {
        
        myfile << "*** @f " + netlists[i].getIndex() << endl;
        myfile << "*** @arity " + to_string(netlists[i].getArity()) << endl;
        myfile << "*** @pos2d " + parsedPositions[i * 2] + " " + parsedPositions[(i * 2) + 1] << endl;
        string  connString = "";
        for (size_t a = 0; a < 4; a++)
        {
            if (connections[i][a] != "")
            {
                connString += connections[i][a];

                if (a < 3)
                    connString += " ";
            }
        }
        myfile << "*** @conn " + connString << endl;

        for (int j = 0; j < (netlists[i].getArity() * 3); j++) {
            if (netlists[i].getInverter(j)) {
                if (j % 3 == 1) {

                    string addInverter = connections[i][(j - 1) / 3] + "_p";
                    if (find(usedInverters.begin(), usedInverters.end(), addInverter) == usedInverters.end()) {
                        myfile << "xpti" << inverterCount << " " << connections[i][(j - 1) / 3] << " " <<
                            addInverter << " vdd pti" << endl;
                        inverterCount++;
                        usedInverters.push_back(addInverter);
                    }
                }
                else if (j % 3 == 2) {
                    string addInverter = connections[i][(j - 2) / 3] + "_n";
                    if (find(usedInverters.begin(), usedInverters.end(), addInverter) == usedInverters.end()) {
                        myfile << "xnti" << inverterCount << " " << connections[i][(j - 2) / 3] << " " <<
                            addInverter << " vdd nti" << endl;
                        inverterCount++;
                        usedInverters.push_back(addInverter);
                    }
                }
            }
        }

        myfile << "\nxckt" << i << " ";

        for (int j = 0; j < 4; j++) {
            if (j < 3) {
                if (netlists[i].getInverter(j * 3)) {
                    myfile << connections[i][j] << " ";
                }
                if (netlists[i].getInverter((j * 3) + 1)) {
                    myfile << connections[i][j] << "_p ";
                }
                if (netlists[i].getInverter((j * 3) + 2)) {
                    myfile << connections[i][j] << "_n ";
                }
            }
        }

        myfile << connections[i][3] << " vdd " << "f_" << netlists[i].getIndex() << "\n\n";

    }

    int j = netlists.size();
    for (int i = 0; i < savedCircuitCount; i++) {

        myfile << "*** @s " + savedCircuitNames[i] << endl;
        myfile << "*** @pos2d " + parsedPositions[(j + i) * 2] + " " + parsedPositions[((j + i) * 2) + 1] << endl;
        string  connString ="";
        for (size_t a = 0; a < connections[j+i].size(); a++)
        {
            connString += connections[j+i][a];
                    
            if (a < (connections[j + i].size() -1))
                connString += " ";
        }
        myfile << "*** @conn " + connString << endl;

        myfile << "\nxckt" << j+i << " ";

        for (int a = 0; a < connections[j + i].size(); a++) {
                myfile << connections[j+i][a] << " ";
        }

        myfile << "vdd " << savedCircuitNames[i] << "\n\n";

    }

    myfile << "\n\n.ends\n\n";
    myfile.close();

    return (inverterCount)*2; //2 transistors per inverter, starting at zero
}


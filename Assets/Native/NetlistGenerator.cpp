
// Initial code written by Halvor Nybø Risto for a student short paper for SIMS 2020
// Modified and extended to be part of the Mixed Radix Circuit Synthesis tool by Steven Bos 
// Research group website: http://www.ternaryresearch.com/
// GPL-3 license

#include <iostream>
#include <vector>
#include <math.h>
#include <fstream>
#include <string>
#include <algorithm> 
#include <functional>
#include <cctype>
#include <stdlib.h>
#include <stdio.h>

#include "common.h"

using namespace std;

vector<char> truthtable;	//the truthtable for the entire circuit
vector<char> tempVect;
vector<vector<char> > networks;	// the four truth tables, pull-up and pull-down networks for 0.9v and 0.45v 
vector<char> upvddgnd;		//network[0][x]
vector<char> downvddgnd;	//network[1][x]
vector<char> uphalfvdd;		//network[2][x]
vector<char> downhalfvdd;	//network[3][x]
vector<vector<vector<string> > > circuit;	//network, group, series. The transistor types and their connections are encoded in this vector
vector<char> mask;	// the rectangular groupings are first generated here, then compared to the truthtable.
vector<vector<char> > groups;	//groupnr, values. The valid rectangular groupings are stored here.
string optimizedIndex;
vector<bool> invArray;
enum Mode { modelA_woBody = 0, modelB_wBodyDividersOnly = 1, modelB_wBodyDividerTransistor};
Mode mode; //0 is model A and without body effect, 1 is model B and with body effect

int dimensions = -1; // the number of inputs
int maskIndex = 0;
int dividersNeeded = 0;

int dimensionLevel(int index, int dimension) {	//returns the level a specific dimension is for a given index (not its value) NOT ZERO INDEXED
	return ((index % int((pow(3, dimension)))) / int(pow(3, (dimension - 1))));
}

void maskRecurs(int n, int p1, int p2) {
	//recursivly goes through all the dimensions an fills in the mask vector between the two opposing corner points
	for (int i = 0; i < 3; i++) {
		if (n == 1) {
			maskIndex += 1;
		}
		if (!(i > dimensionLevel(p2, n)) && !(i < dimensionLevel(p1, n))) {	//"current point" not smaller than p1,  
			if (n > 1) {													//not bigger than p2 in the current dimension
				maskRecurs(n - 1, p1, p2);
			}
			else {
				mask[maskIndex - 1] = '1';
			}
		}
		else {
			if (n > 1) maskIndex += int(pow(3, n - 1));

		}

	}
}

void drawMask(int p1, int p2) {		// draws an n-dimensional rectangle between two corner points
	fill(mask.begin(), mask.end(), '0');
	maskIndex = 0;
	bool error = false;
	for (int i = 1; i < dimensions + 1; i++) {
		if (dimensionLevel(p2, i) < dimensionLevel(p1, i)) {
			error = true;
		}
	}

	if (error) {
		//cout << "\nError: one of p2's dimensions is lower than p1's.\n"; // The starting corner of the rectangle must be smaller in all dimensions compared to the end corner

	}
	else {
		maskRecurs(dimensions, p1, p2); // calls the recursive function to draw the rectangle in the mask vector
	}
}


LIBRARY_API void SetMode(int newMode) {
	mode = static_cast<Mode>(newMode);
}

LIBRARY_API int* GetOptimzedTT() {
	int* tt = new int[truthtable.size()];

	for (size_t i = 0; i < truthtable.size(); i++)
	{
		switch (truthtable[i])
		{
		case '0':
			tt[i] = 0;
			break;
		case '1':
			tt[i] = 1;
			break;
		case '2':
			tt[i] = 2;
			break;
		}
	}

	return tt;
}

LIBRARY_API void GetOptimzedTT_Release(int* pArray)
{
	delete[] pArray;
}

LIBRARY_API int* GetInvArray() {
	int* tt = new int[invArray.size()];

	for (size_t i = 0; i < invArray.size(); i++)
	{
		if (invArray[i])
		{
			tt[i] = 1;
		}
		else
		{
			tt[i] = 0;
		}
	}

	return tt;
}

LIBRARY_API void GetInvArray_Release(int* pArray)
{
	delete[] pArray;
}

//this is a test function
LIBRARY_API int TestSum(int* ttFromUnity, int ttFromUnityLength) {

	int sum = 0;
	for (int i = 0; i < ttFromUnityLength; ++i)
		sum += ttFromUnity[i];

	return sum;
}

LIBRARY_API int CreateNetlist(int newMode, char* filePath, int* ttFromUnity, int ttFromUnityLength, int arity) {

/////////////////
//Stage0: INIT
////////////////
	//clear arrays
	circuit.clear();
	truthtable.clear();
	tempVect.clear();
	networks.clear();
	mask.clear();
	groups.clear();
	maskIndex = 0;
	invArray.clear();

	dimensions = arity; //or three, derive from function name
	
	//convert int to Mode
	mode = static_cast<Mode>(newMode);
	
	int mysteryNumber = dimensions * dimensions * 100;//dimensions * 1000;	// This number must be higher for more inputs. Program will crash if it is too low. Must be higher than number of groups found.
	int mysteryExponent = 1;		// NOTE: The author is not happy with the use of these mystery numbers. However it will do for now.
	
	circuit.resize(4, vector<vector<string> >(mysteryNumber, vector<string>(dimensions)));

	for (int i = 0; i < pow(3, dimensions); i++) {
		truthtable.push_back('0');
		tempVect.push_back('0');
		networks.resize(4);
		networks[0].push_back('0');
		networks[1].push_back('0');
		networks[2].push_back('0');
		networks[3].push_back('0');
		mask.push_back('0');

		groups.resize(int(pow(pow(3, dimensions), mysteryExponent)));
		for (int j = 0; j < int(pow(pow(3, dimensions), mysteryExponent)); j++) {
			groups[j].push_back('0');
		}
	}

	/////////////////
	//Stage1: Fill truthtable
	////////////////

	for (size_t i = 0; i < ttFromUnityLength; i++)
	{
		switch (ttFromUnity[i])
		{
		case 0:
			truthtable[i] = '0';
			break;
		case 1:
			truthtable[i] = '1';
			break;
		case 2:
			truthtable[i] = '2';
			break;
		case 3:
			truthtable[i] = 'x';
			break;
		}
	}

	

	/////////////////
	//Stage2: Generates the truthtables for the 4 transistor networks based on the full truthtable
	////////////////

	for (int i = 0; i < truthtable.size(); i++) {

		if (truthtable[i] == 'x') {
			networks[0][i] = 'x';
			networks[1][i] = 'x';
			networks[2][i] = 'x';
			networks[3][i] = 'x';
		}
		if (truthtable[i] == '0') {
			networks[0][i] = '0';
			networks[1][i] = '1';
			networks[2][i] = '0';
			networks[3][i] = 'x';
		}
		if (truthtable[i] == '1') {
			networks[0][i] = '0';
			networks[1][i] = '0';
			networks[2][i] = '1';
			networks[3][i] = '1';
		}
		if (truthtable[i] == '2') {
			networks[0][i] = '1';
			networks[1][i] = '0';
			networks[2][i] = 'x';
			networks[3][i] = '0';
		}
	}

	fill(truthtable.begin(), truthtable.end(), '0'); // empties the full truthtable for later use

	//DEBUG
	//for (int n = 0; n < 4; n++) {
	//	cout << "\n\nNETWORK " << n << ": \n";
	//	for (int i = 0; i < truthtable.size(); i++) {
	//		if (i % 3 == 0) cout << "\n";
	//		if (i % 9 == 0) cout << "\n\n";
	//		cout << networks[n][i];
	//	}

	/////////////////
	//Stage3: ?? generating final circuit truthtable?
	////////////////
	int groupNr = 0;
	bool lessthan = false; // if p2 is lower in any dimension than p1, it is not a valid rectangle
	bool directConnection[4];

	for (int n = 0; n < 4; n++) {
		// For each of the 4 network, a set of optimal groupings of 1s are found. 
		// Each grouping represents a transistor-path towards the output.
		//if (n == 0) cout << "\nBuilding the 0.9V pull-up circuit...\n";
		//if (n == 1) cout << "\nBuilding the 0.9V pull-down circuit...\n";
		//if (n == 2) cout << "\nBuilding the 0.45V pull-up circuit...\n";
		//if (n == 3) cout << "\nBuilding the 0.45V pull-down circuit...\n";

		groupNr = 0;
		for (int f = 0; f < truthtable.size(); f++) {
			fill(groups[f].begin(), groups[f].end(), '0');
		}
		for (int p1 = 0; p1 < truthtable.size(); p1++) { //for each point in the network which is 1 or x

			if ((networks[n][p1] == '1') || (networks[n][p1] == 'x')) {
				for (int p2 = p1; p2 < truthtable.size(); p2++) { //  for each point after the 1 or x                 
					lessthan = false;

					for (int j = 1; j < dimensions + 1; j++) {					// check if it's lower in any dimension (it would result in a 0-mask)
						if (dimensionLevel(p2, j) < dimensionLevel(p1, j)) {
							lessthan = true;
						}
					}

					if (!lessthan) {			// if it is a valid rectangle, compare it with the truthtable of the network
						drawMask(p1, p2);
						bool equalMask = true;
						for (int j = p1; j < p2 + 1; j++) {		// for every point, see if a 1 in the mask is a 0 in the network truth table
							if (mask[j] == '1') {
								if (networks[n][j] == '0') {
									equalMask = false;
								}
							}
						}

						if (equalMask == true) {				// if there are no 0s compared to the mask
							bool written = false;
							bool covered = true;

							for (int g = 0; g < groupNr; g++) {				// check if a group would be covered by the next group, and overwrite it if it does 
																			//NOTE: This can overwrite multiple groups, resulting in duplicate groups
								covered = true;
								for (int c = 0; c < p2; c++) {
									if ((groups[g][c] == '1') && (mask[c] == '0')) {
										covered = false;
									}
								}
								if (covered) {
									for (int j = p1; j < p2 + 1; j++) {
										groups[g][j] = mask[j];
									}
									written = true;
								}
							}

							if (!written) {

								fill(tempVect.begin(), tempVect.end(), '0');			// checks if the sum of the preexisting groups would cover the mask
								for (int g = 0; g < groupNr; g++) {
									for (int j = 0; j < truthtable.size(); j++) {
										if (groups[g][j] == '1') {
											tempVect[j] = '1';
										}
									}
								}

								covered = true;
								for (int j = 0; j < truthtable.size(); j++) {
									if (mask[j] == '1') {
										if (tempVect[j] == '0') {
											covered = false;
										}
									}
								}
								if (!covered) {
									for (int j = p1; j < p2 + 1; j++) {
										groups[groupNr][j] = mask[j];
									}
									groupNr += 1;
									//cout << ".";
								}
							}

						}
					}
				}
			}
		}

		bool duplicate = false;

		for (int g = groupNr - 1; g > 0; g--) {				// checks for duplicate groups
			for (int g2 = 0; g2 < g; g2++) {
				duplicate = true;
				for (int c = 0; c < truthtable.size(); c++) {
					if ((groups[g2][c] == '1') && (groups[g][c] == '0')) {
						duplicate = false;
					}
				}
			}

			if (duplicate) {
				for (int j = 0; j < truthtable.size(); j++) {
					groups[g][j] = '0';
				}
				//for () // shift all groups above this one down and subtract groupNr (not implemented)	

			}

		}


		//for each group, see if the sum of the other groups would cover all the 1s (WITHOUT x)
		for (int i = 0; i < groupNr; i++) {
			fill(tempVect.begin(), tempVect.end(), '0');
			for (int j = 0; j < groupNr; j++) {
				if (i != j) {
					for (int k = 0; k < truthtable.size(); k++) {
						if (groups[j][k] == '1') {
							tempVect[k] = '1';

						}
					}


				}
			}

			bool covered = true;
			for (int k = 0; k < truthtable.size(); k++) {
				if ((networks[n][k] == '1') && (tempVect[k] != '1')) {
					covered = false;
				}
			}
			if (covered) {	// if the group is not needed, set it to 0
				for (int k = 0; k < truthtable.size(); k++) {
					groups[i][k] = '0';
				}
			}
		}


		fill(tempVect.begin(), tempVect.end(), '0');
		for (int j = 0; j < groupNr; j++) {

			for (int k = 0; k < truthtable.size(); k++) {
				if (groups[j][k] == '1') {
					tempVect[k] = '1';

				}
			}

		}


		// reconstructing the final full truthtable (if it had x's in it to begin with, it don't anymore!)
		for (int i = 0; i < truthtable.size(); i++) {
			if (tempVect[i] == '1') {
				if (n == 0) {
					truthtable[i] = '2';
				}
				else if (n == 1) {
					truthtable[i] = '0';
				}
				else if (n == 2) {
					if (truthtable[i] != '2') {
						truthtable[i] = '1';
					}

				}
				//else if (n == 3) {
				// don't need this one, it's covered by the others
				//}
			}
		}


		// build the circuit
		for (int g = 0; g < groupNr; g++) {
			for (int d = 0; d < dimensions; d++) {

				bool cut = true;
				string transType = "111"; //transtype represents the types of transistors. For example, "100" is open for low voltage, but not for medium or high
											// "010" is a connection of "110" and "011" in series
				// the circuit is build by covering every group with the use of these transistor types
				for (int L = 0; L < 3; L++) {
					for (int i = 0; i < truthtable.size(); i++) {
						if (dimensionLevel(i, d + 1) == L) {
							if (groups[g][i] != '0') {
								cut = false;
							}
						}
					}
					if (cut) {
						transType[L] = '0';
					}
					else cut = true;
				}
				circuit[n][g][d] = transType;
			}
		}

		directConnection[n] = true;
		for (int i = 0; i < truthtable.size(); i++) {
			if (groups[0][i] != '1') { directConnection[n] = false; }
		}
	}


	/* NOTE:
	The circuit can be optmized further at this point.
	If two transistors of the same type and input in the same network are both connected to vdd on one side, they can be merged.
	The spaces along VDD, GND, OUT can hold mergings, which then produces sub-spaces for further merging.
	This optimization was not implemented here.
	(the order of transistors in each branch can be swapped around for maximum optimization)
	high-arity functions can be optimized more than low-arity functions
	(The high arity functions are highly unoptimized in this synthesizer)
	*/

	//DEBUG
	/*cout << "\n\n final circuit truthtable: \n";

	for (int i = 0; i < truthtable.size(); i++) {
		if (i % 3 == 0) cout << "\n";
		if (i % 9 == 0) cout << "\n";
		if (i % 27 == 0) cout << "\n";
		cout << truthtable[i];
	}*/

	/////////////////
	//Stage4: Discover Hept index based on values (reverse lookup)
	////////////////

	// THE BASE-27 HEPTAVINTIMAL NOTATION
	// 000 001 002 010 011 012 020 021 022 100 101 102 110 111 112 120 121 122 200 201 202 210 211 212 220 221 222
	//  0	1	2	3	4	5	6	7	8	9	A	B	C	D	E	F	G	H	K	M	N	P	R	T	V	X	Z
	optimizedIndex = "";
	string hept;
	for (int i = truthtable.size() - 1; i > 0; i -= 3) { // the heptavintimal function index is generated
		hept = truthtable[i];
		hept += truthtable[i - 1];
		hept += truthtable[i - 2];
		if (hept == "000") { optimizedIndex += "0"; }
		else if (hept == "001") { optimizedIndex += "1"; }
		else if (hept == "002") { optimizedIndex += "2"; }
		else if (hept == "010") { optimizedIndex += "3"; }
		else if (hept == "011") { optimizedIndex += "4"; }
		else if (hept == "012") { optimizedIndex += "5"; }
		else if (hept == "020") { optimizedIndex += "6"; }
		else if (hept == "021") { optimizedIndex += "7"; }
		else if (hept == "022") { optimizedIndex += "8"; }
		else if (hept == "100") { optimizedIndex += "9"; }
		else if (hept == "101") { optimizedIndex += "A"; }
		else if (hept == "102") { optimizedIndex += "B"; }
		else if (hept == "110") { optimizedIndex += "C"; }
		else if (hept == "111") { optimizedIndex += "D"; }
		else if (hept == "112") { optimizedIndex += "E"; }
		else if (hept == "120") { optimizedIndex += "F"; }
		else if (hept == "121") { optimizedIndex += "G"; }
		else if (hept == "122") { optimizedIndex += "H"; }
		else if (hept == "200") { optimizedIndex += "K"; }
		else if (hept == "201") { optimizedIndex += "M"; }
		else if (hept == "202") { optimizedIndex += "N"; }
		else if (hept == "210") { optimizedIndex += "P"; }
		else if (hept == "211") { optimizedIndex += "R"; }
		else if (hept == "212") { optimizedIndex += "T"; }
		else if (hept == "220") { optimizedIndex += "V"; }
		else if (hept == "221") { optimizedIndex += "X"; }
		else if (hept == "222") { optimizedIndex += "Z"; }
	}

	//DEBUG
	//cout << "\nheptavintimal function index: " << index;


	/////////////////
	//Stage5: Setup parameters for Netlist file and create directory
	////////////////

	string filename;
	filename = "f_";
	for (int i = 0; i < (int(pow(3, dimensions - 1))) - optimizedIndex.length(); i++) { filename += "0"; }
	filename += optimizedIndex;

	
	/////////////////
	//Stage6: Create netlist file
	////////////////

	ofstream myfile;
	string path = filePath;
	path += filename;
	path += ".sp";
	myfile.open(path);




	// SPECIFY TRANSISTOR MODEL AND PARAMETERS HERE 
	string p0 = " gnd PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 \n+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 13 "; //" ptype 1.018nm";
	string n0 = " gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 \n+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 13 "; //" ntype 1.018nm";

	string n1 = " gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 \n+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 10 "; //" ntype 0.783nm";
	string n2 = " gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 \n+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19 "; //" ntype 1.487nm";

	string p1 = " gnd PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 \n+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 10  "; //" ptype 0.783nm";
	string p2 = " gnd PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 \n+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19  "; //" ptype 1.487nm";



	myfile << ".subckt " << filename << " "; //<< " i0 i0_p i0_n i1 i1_p i1_n out vdd\n"; // circuit relies on external PTI and NTI

	for (int i = 0; i < dimensions; i++) {	 // CREATING THE SUBCIRCUIT INTERFACE. will only require PTI and NTI when necessary
		bool bI = false;
		bool bIP = false;
		bool bIN = false;
		for (int n = 0; n < 4; n++) {
			for (int g = 0; g < mysteryNumber; g++) {
				if (n % 2 == 0) {
					if (circuit[n][g][i] == "100" || circuit[n][g][i] == "110" || circuit[n][g][i] == "010") {
						bI = true;
					}
					if (circuit[n][g][i] == "001") {
						bIP = true;
					}
					if (circuit[n][g][i] == "011" || circuit[n][g][i] == "010") {
						bIN = true;
					}
				}
				else {
					if (circuit[n][g][i] == "001" || circuit[n][g][i] == "011" || circuit[n][g][i] == "010") {
						bI = true;
					}
					if (circuit[n][g][i] == "110" || circuit[n][g][i] == "010") {
						bIP = true;
					}
					if (circuit[n][g][i] == "100") {
						bIN = true;
					}
				}

			}
		}
		if (bI) myfile << "i" << i << " ";
		if (bIP) myfile << "i" << i << "_p ";
		if (bIN) myfile << "i" << i << "_n ";

		invArray.push_back(bI);
		invArray.push_back(bIP);
		invArray.push_back(bIN);
	}

	//in case there should be a direct connection within a transistor network, it's connected without transistors 
	if (directConnection[0]) {
		myfile << "vdd "; //output is VDD
	}
	else if (directConnection[1]) {
		myfile << "gnd "; //output is VDD
	}
	else {
		myfile << "out ";
	}

	myfile << "vdd\n"; // end of inputs/outputs interface

	int transistors = 0;

	//check if dividers are needed
	//assume it is false, until proven otherwise by going over pull down and pull up networks and decide if they have only "000" meaning empty
	dividersNeeded = 0;
	for (size_t g = 0; g < mysteryNumber; g++)
	{
		for (int dd = 0; dd < dimensions; dd++) {

			//check pull-up half
			if (circuit[2][g][dd] != "000" && circuit[2][g][dd] != "111" && !circuit[2][g][dd].empty())
			{
				dividersNeeded = 1;
			}

			//check pull-down half
			if (circuit[3][g][dd] != "000" && circuit[3][g][dd] != "111" && !circuit[3][g][dd].empty())
			{
				dividersNeeded = 1;
			}
		}
	}

	switch (mode)
	{
	case Mode::modelA_woBody:
	{
		if (dividersNeeded)
		{
			myfile << "\n\nxp0 ";
			if (directConnection[2]) {
				myfile << "vdd "; //connects "up" to VDD
			}
			else { myfile << "up "; }
			myfile << "out out" << p0;

			myfile << "\nxn1 ";
			myfile << "out out ";
			if (directConnection[3]) {
				myfile << "gnd "; //connects "down" to GND
			}
			else { myfile << "down"; }
			myfile << n0 << "\n";

			transistors = 2; //counts number of transistors
		}

		int connections = 0; //counts number of connection nodes
		string connect1 = ""; // connection variables (these depend on the network and group number)
		string connect2 = "";
		string connect3 = "";
		string out = "";		// the connection to the output (out, up, down)
		string vsource = "";	// the first connection (gnd, vdd)

		for (int n = 0; n < 4; n++) {


			if (n == 0) {
				out = "out";
				vsource = "vdd";
				myfile << "\n\n***pullup full" << endl;
			}
			if (n == 1) {
				out = "out";
				vsource = "gnd";
				myfile << "\n\n***pulldown full" << endl;
			}
			if (n == 2) {
				out = "up";
				vsource = "vdd";
				myfile << "\n\n***pullup half" << endl;
			}

			if (n == 3) {
				out = "down";
				vsource = "gnd";
				myfile << "\n\n***pulldown half" << endl;
			}

			if (directConnection[n]) {
				myfile << "\n***Direct Connection\n";
			}

			for (int g = 0; g < mysteryNumber; g++) {
				// the first and last groups to be implemented indicates when the connections should be at vsource and out
				// in circuit[n][g][d], what number d is the first and last valid one? (empty, 111, 000 are not valid)
				int firstDimension = -1;
				int lastDimension = 0;
				for (int dd = 0; dd < dimensions; dd++) {

					if (circuit[n][g][dd] != "000" && circuit[n][g][dd] != "111" && !circuit[n][g][dd].empty()) {
						lastDimension = dd;
						if (firstDimension == -1) {
							firstDimension = dd;
						}
					}
				}

				for (int d = 0; d < dimensions; d++) {
					if (!circuit[n][g][d].empty() && circuit[n][g][d] != "000" && circuit[n][g][d] != "111") {

						// connection variables are defined
						if (d == firstDimension) {
							myfile << "\n";
							connect1 = vsource;
						}
						else {
							connect1 = 'p' + to_string(connections);
							connections += 1;
						}

						if (d == lastDimension) {
							if (circuit[n][g][d] == "010") {
								connect2 = 'p' + to_string(connections);
								connections += 1;
								connect3 = out;
							}
							else {
								connect2 = out;
							}
						}
						else {
							connect2 = 'p' + to_string(connections);
							if (circuit[n][g][d] == "010") {
								connections += 1;
								connect3 = 'p' + to_string(connections);

							}
						}

						// circuit is built using the "transistor types" in the circuit vector and the connection variables
						if (n % 2 == 0) {
							if (circuit[n][g][d] == "100") {	// small ptype I
								myfile << "\nxp" << transistors << " " << connect1 << " i" << d << " " << connect2 << p1;
								transistors += 1;
							}
							if (circuit[n][g][d] == "110") {	// big ptype I
								myfile << "\nxp" << transistors << " " << connect1 << " i" << d << " " << connect2 << p2;
								transistors += 1;
							}
							if (circuit[n][g][d] == "001") {	// big ptype I_P
								myfile << "\nxp" << transistors << " " << connect1 << " i" << d << "_p " << connect2 << p2;
								transistors += 1;
							}
							if (circuit[n][g][d] == "011") {	// big ptype I_N
								myfile << "\nxp" << transistors << " " << connect1 << " i" << d << "_n " << connect2 << p2;
								transistors += 1;
							}
							if (circuit[n][g][d] == "010") {	// big ptype I + big ptype I_N
								myfile << "\nxp" << transistors << " " << connect1 << " i" << d << " " << connect2 << p2;
								transistors += 1;
								myfile << "\nxp" << transistors << " " << connect2 << " i" << d << "_n " << connect3 << p2;
								transistors += 1;
							}
						}
						else {
							if (circuit[n][g][d] == "100") {	// big ntype I_N
								myfile << "\nxn" << transistors << " " << connect1 << " i" << d << "_n " << connect2 << n2;
								transistors += 1;
							}
							if (circuit[n][g][d] == "110") {	// big ntype I_P
								myfile << "\nxn" << transistors << " " << connect1 << " i" << d << "_p " << connect2 << n2;
								transistors += 1;
							}
							if (circuit[n][g][d] == "001") {	// small ntype I
								myfile << "\nxn" << transistors << " " << connect1 << " i" << d << " " << connect2 << n1;
								transistors += 1;
							}
							if (circuit[n][g][d] == "011") {	// big ntype I
								myfile << "\nxn" << transistors << " " << connect1 << " i" << d << " " << connect2 << n2;
								transistors += 1;
							}

							if (circuit[n][g][d] == "010") {	// big ntype I_P + big ntype I
								myfile << "\nxn" << transistors << " " << connect1 << " i" << d << "_p " << connect2 << n2;
								transistors += 1;
								myfile << "\nxn" << transistors << " " << connect2 << " i" << d << " " << connect3 << n2;
								transistors += 1;
							}
						}
					}
				}
			}
		}

	}
	break;
	case Mode::modelB_wBodyDividersOnly:
	{
		//slightly adjust to accomodate for body effect
		p0 = " vdd PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 \n+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 13 "; //" ptype 1.018nm";
		
		//if (n == 0) cout << "\nBuilding the 0.9V pull-up circuit...\n";
		//if (n == 1) cout << "\nBuilding the 0.9V pull-down circuit...\n";
		//if (n == 2) cout << "\nBuilding the 0.45V pull-up circuit...\n";
		//if (n == 3) cout << "\nBuilding the 0.45V pull-down circuit...\n";
		
		if (dividersNeeded)
		{
			myfile << "\n\nxp0 ";
			if (directConnection[0]) //pull up full
			{
				myfile << "vdd ";
			}
			else if (directConnection[3]) //pull down half
			{
				myfile << "gnd ";
			}
			else
			{
				myfile << "up ";
			}
			myfile << "gnd out" << p0;

			myfile << "\nxn1 ";
			if (directConnection[1]) //pull down full
			{
				myfile << "gnd ";
			}
			else if (directConnection[2]) //pull up half
			{
				myfile << "vdd ";
			}
			else
			{
				myfile << "down ";
			}
			myfile << "vdd out" << n0 << "\n";

			transistors = 2; //counts number of transistors
		}

		int connections = 0; //counts number of connection nodes
		string connect1 = ""; // connection variables (these depend on the network and group number)
		string connect2 = "";
		string connect3 = "";
		string out = "";		// the connection to the output (out, up, down)
		string vsource = "";	// the first connection (gnd, vdd)

		for (int n = 0; n < 4; n++) {


			if (n == 0) {
				out = "up";
				vsource = "vdd";
				myfile << "\n\n***pullup full" << endl;
			}
			if (n == 1) {
				out = "down";
				vsource = "gnd";
				myfile << "\n\n***pulldown full" << endl;
			}
			if (n == 2) {
				out = "down";
				vsource = "vdd";
				myfile << "\n\n***pullup half" << endl;
			}

			if (n == 3) {
				out = "up";
				vsource = "gnd";
				myfile << "\n\n***pulldown half" << endl;
			}

			if (directConnection[n]) {
				myfile << "\n***Direct Connection\n";
			}

			for (int g = 0; g < mysteryNumber; g++) {
				// the first and last groups to be implemented indicates when the connections should be at vsource and out
				// in circuit[n][g][d], what number d is the first and last valid one? (empty, 111, 000 are not valid)
				int firstDimension = -1;
				int lastDimension = 0;
				for (int dd = 0; dd < dimensions; dd++) {

					if (circuit[n][g][dd] != "000" && circuit[n][g][dd] != "111" && !circuit[n][g][dd].empty()) {
						lastDimension = dd;
						if (firstDimension == -1) {
							firstDimension = dd;
						}
					}
				}

				for (int d = 0; d < dimensions; d++) {
					if (!circuit[n][g][d].empty() && circuit[n][g][d] != "000" && circuit[n][g][d] != "111") {

						// connection variables are defined
						if (d == firstDimension) {
							myfile << "\n";
							connect1 = vsource;
						}
						else {
							connect1 = 'p' + to_string(connections);
							connections += 1;
						}

						if (d == lastDimension) {
							if (circuit[n][g][d] == "010") {
								connect2 = 'p' + to_string(connections);
								connections += 1;
								connect3 = out;
							}
							else {
								connect2 = out;
							}
						}
						else {
							connect2 = 'p' + to_string(connections);
							if (circuit[n][g][d] == "010") {
								connections += 1;
								connect3 = 'p' + to_string(connections);

							}
						}

						// circuit is built using the "transistor types" in the circuit vector and the connection variables
						if (n % 2 == 0) {
							if (circuit[n][g][d] == "100") {	// small ptype I
								myfile << "\nxp" << transistors << " " << connect1 << " i" << d << " " << connect2 << p1;
								transistors += 1;
							}
							if (circuit[n][g][d] == "110") {	// big ptype I
								myfile << "\nxp" << transistors << " " << connect1 << " i" << d << " " << connect2 << p2;
								transistors += 1;
							}
							if (circuit[n][g][d] == "001") {	// big ptype I_P
								myfile << "\nxp" << transistors << " " << connect1 << " i" << d << "_p " << connect2 << p2;
								transistors += 1;
							}
							if (circuit[n][g][d] == "011") {	// big ptype I_N
								myfile << "\nxp" << transistors << " " << connect1 << " i" << d << "_n " << connect2 << p2;
								transistors += 1;
							}
							if (circuit[n][g][d] == "010") {	// big ptype I + big ptype I_N
								myfile << "\nxp" << transistors << " " << connect1 << " i" << d << " " << connect2 << p2;
								transistors += 1;
								myfile << "\nxp" << transistors << " " << connect2 << " i" << d << "_n " << connect3 << p2;
								transistors += 1;
							}
						}
						else {
							if (circuit[n][g][d] == "100") {	// big ntype I_N
								myfile << "\nxn" << transistors << " " << connect1 << " i" << d << "_n " << connect2 << n2;
								transistors += 1;
							}
							if (circuit[n][g][d] == "110") {	// big ntype I_P
								myfile << "\nxn" << transistors << " " << connect1 << " i" << d << "_p " << connect2 << n2;
								transistors += 1;
							}
							if (circuit[n][g][d] == "001") {	// small ntype I
								myfile << "\nxn" << transistors << " " << connect1 << " i" << d << " " << connect2 << n1;
								transistors += 1;
							}
							if (circuit[n][g][d] == "011") {	// big ntype I
								myfile << "\nxn" << transistors << " " << connect1 << " i" << d << " " << connect2 << n2;
								transistors += 1;
							}

							if (circuit[n][g][d] == "010") {	// big ntype I_P + big ntype I
								myfile << "\nxn" << transistors << " " << connect1 << " i" << d << "_p " << connect2 << n2;
								transistors += 1;
								myfile << "\nxn" << transistors << " " << connect2 << " i" << d << " " << connect3 << n2;
								transistors += 1;
							}
						}
					}
				}
			}
		}

	}
	break;
	case Mode::modelB_wBodyDividerTransistor:
	{
		//slightly adjust to accomodate for body effect
		p0 = " vdd PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 \n+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 13 "; //" ptype 1.018nm";

		n1 = " NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 \n+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 10 "; //" ntype 0.783nm";
		n2 = " NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 \n+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19 "; //" ntype 1.487nm";

		p1 = " PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 \n+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 10  "; //" ptype 0.783nm";
		p2 = " PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 \n+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19  "; //" ptype 1.487nm";

		//if (n == 0) cout << "\nBuilding the 0.9V pull-up circuit...\n";
		//if (n == 1) cout << "\nBuilding the 0.9V pull-down circuit...\n";
		//if (n == 2) cout << "\nBuilding the 0.45V pull-up circuit...\n";
		//if (n == 3) cout << "\nBuilding the 0.45V pull-down circuit...\n";

		if (dividersNeeded)
		{
			myfile << "\n\nxp0 ";
			if (directConnection[0]) //pull up full
			{
				myfile << "vdd ";
			}
			else if (directConnection[3]) //pull down half
			{
				myfile << "gnd ";
			}
			else
			{
				myfile << "up ";
			}
			myfile << "gnd out" << p0;

			myfile << "\nxn1 ";
			if (directConnection[1]) //pull down full
			{
				myfile << "gnd ";
			}
			else if (directConnection[2]) //pull up half
			{
				myfile << "vdd ";
			}
			else
			{
				myfile << "down ";
			}
			myfile << "vdd out" << n0 << "\n";

			transistors = 2; //counts number of transistors
		}

		int connections = 0; //counts number of connection nodes
		string connect1 = ""; // connection variables (these depend on the network and group number)
		string connect2 = "";
		string connect3 = "";
		string out = "";		// the connection to the output (out, up, down)
		string vsource = "";	// the first connection (gnd, vdd)

		for (int n = 0; n < 4; n++) {


			if (n == 0) {
				out = "up";
				vsource = "vdd";
				myfile << "\n\n***pullup full" << endl;
			}
			if (n == 1) {
				out = "down";
				vsource = "gnd";
				myfile << "\n\n***pulldown full" << endl;
			}
			if (n == 2) {
				out = "down";
				vsource = "vdd";
				myfile << "\n\n***pullup half" << endl;
			}

			if (n == 3) {
				out = "up";
				vsource = "gnd";
				myfile << "\n\n***pulldown half" << endl;
			}

			if (directConnection[n]) {
				myfile << "\n***Direct Connection\n";
			}

			for (int g = 0; g < mysteryNumber; g++) {
				// the first and last groups to be implemented indicates when the connections should be at vsource and out
				// in circuit[n][g][d], what number d is the first and last valid one? (empty, 111, 000 are not valid)
				int firstDimension = -1;
				int lastDimension = 0;
				for (int dd = 0; dd < dimensions; dd++) {

					if (circuit[n][g][dd] != "000" && circuit[n][g][dd] != "111" && !circuit[n][g][dd].empty()) {
						lastDimension = dd;
						if (firstDimension == -1) {
							firstDimension = dd;
						}
					}
				}

				for (int d = 0; d < dimensions; d++) {
					if (!circuit[n][g][d].empty() && circuit[n][g][d] != "000" && circuit[n][g][d] != "111") {

						// connection variables are defined
						if (d == firstDimension) {
							myfile << "\n";
							connect1 = vsource;
						}
						else {
							connect1 = 'p' + to_string(connections);
							connections += 1;
						}

						if (d == lastDimension) {
							if (circuit[n][g][d] == "010") {
								connect2 = 'p' + to_string(connections);
								connections += 1;
								connect3 = out;
							}
							else {
								connect2 = out;
							}
						}
						else {
							connect2 = 'p' + to_string(connections);
							if (circuit[n][g][d] == "010") {
								connections += 1;
								connect3 = 'p' + to_string(connections);

							}
						}

						// circuit is built using the "transistor types" in the circuit vector and the connection variables
						if (n % 2 == 0) {
							if (circuit[n][g][d] == "100") {	// small ptype I
								myfile << "\nxp" << transistors << " " << connect1 << " i" << d << " " << connect2 << " " << connect1 << p1;
								transistors += 1;
							}
							if (circuit[n][g][d] == "110") {	// big ptype I
								myfile << "\nxp" << transistors << " " << connect1 << " i" << d << " " << connect2 << " " << connect1 << p2;
								transistors += 1;
							}
							if (circuit[n][g][d] == "001") {	// big ptype I_P
								myfile << "\nxp" << transistors << " " << connect1 << " i" << d << "_p " << connect2 << " " << connect1 << p2;
								transistors += 1;
							}
							if (circuit[n][g][d] == "011") {	// big ptype I_N
								myfile << "\nxp" << transistors << " " << connect1 << " i" << d << "_n " << connect2 << " " << connect1 << p2;
								transistors += 1;
							}
							if (circuit[n][g][d] == "010") {	// big ptype I + big ptype I_N
								myfile << "\nxp" << transistors << " " << connect1 << " i" << d << " " << connect2 << " " << connect1 << p2;
								transistors += 1;
								myfile << "\nxp" << transistors << " " << connect2 << " i" << d << "_n " << connect3 << " " << connect2 << p2;
								transistors += 1;
							}
						}
						else {
							if (circuit[n][g][d] == "100") {	// big ntype I_N
								myfile << "\nxn" << transistors << " " << connect1 << " i" << d << "_n " << connect2 << " " << connect1 << n2;
								transistors += 1;
							}
							if (circuit[n][g][d] == "110") {	// big ntype I_P
								myfile << "\nxn" << transistors << " " << connect1 << " i" << d << "_p " << connect2 << " " << connect1 << n2;
								transistors += 1;
							}
							if (circuit[n][g][d] == "001") {	// small ntype I
								myfile << "\nxn" << transistors << " " << connect1 << " i" << d << " " << connect2 << " " << connect1 << n1;
								transistors += 1;
							}
							if (circuit[n][g][d] == "011") {	// big ntype I
								myfile << "\nxn" << transistors << " " << connect1 << " i" << d << " " << connect2 << " " << connect1 << n2;
								transistors += 1;
							}

							if (circuit[n][g][d] == "010") {	// big ntype I_P + big ntype I
								myfile << "\nxn" << transistors << " " << connect1 << " i" << d << "_p " << connect2 << " " << connect1 << n2;
								transistors += 1;
								myfile << "\nxn" << transistors << " " << connect2 << " i" << d << " " << connect3 << " " << connect2 << n2;
								transistors += 1;
							}
						}
					}
				}
			}
		}

	}
	break;
	default:
		break;
	}

	
	myfile << "\n\n.ends\n\n";
	myfile.close();

	//cout << "\n\n Circuit outputted into functions/" << filename << ".sp\n\n";

	//system("pause");

	return transistors;
}



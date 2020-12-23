#include <iostream>
#include <string>

using namespace std;

int table[9] = //Logic table outputs. It is also the function index expressed in ternary.
{
	//in2   2,1,0	| in1	
			0,0,0 , //2
			0,0,0 , //1
			0,0,0   //0
};


int in1 = 0;
int in2 = 0;
int index = 0;


int getValue(string prompt, int low, int high) {	//prints a prompt, gets a value (int) between low and high, and returns it. The input value is clamped to the limits.
	int value = 0;
	do {
		if (!cin.good()) {
			cin.clear();
			cin.ignore(INT_MAX, '\n');
		}
		cout << prompt;
		cin >> value;
	} while (!cin.good());


	if (value > high) value = high;
	if (value < low) value = low;
	return value;
}

void printFunction(int index) {
	cout << "-------------------\nfunction of index " << index << endl;
	for (int i = 0; i < 9; i++) table[i] = 0;
	int convert = index;
	for (int i = 9; i > 0; i--) {
		table[i] = convert % 3;
		convert = convert / 3;
	}
	for (int row = 9; row > 0; row--) {	//prints all possible inputs and the corresponding output of function of index i					
		cout << "(" << (9 - row) / 3 << "," << (9 - row) % 3 << ") => " << table[row] << endl;
	}

	//for (int i = 0; i < 9; i++) table[i] = 0; //functions calling this function might need the content of the table
	cout << "-------------------\n\n";
}

int iterateFunctions() {	//prints all function logic tables iteratively. This is not a useful function other than for demonstrating the indexing.
	for (int i = 0; i < 9; i++) table[i] = 0;
	for (int i = 0; i < 19683; i++) {

		printFunction(i);

		/*
		for (int row = 0; row < 9; row++) {	//prints all possible inputs and the corresponding output of function of index i
			cout << "(" << row / 3 << "," << row % 3 << ") => " << table[row] << endl;
		}
		table[0]++;
		for (int digit = 0; digit < 9; digit++) {	//increments the function index / logic table output
			if (table[digit] > 2) {
				table[digit] = 0;
				table[digit + 1]++;
			}
		}
		*/ //interesting but unnecessary given the printFunction function

		char yn;
		do {
			if (!cin.good()) {
				cin.clear();
				cin.ignore(INT_MAX, '\n');
			}

			cout << "continue? y/n: ";
			cin >> yn;
		} while (!cin.good());

		if (yn == 'n') {
			for (int i = 0; i < 9; i++) table[i] = 0;
			return 0;
		}

	}
	for (int i = 0; i < 9; i++) table[i] = 0;
	return 0;
}

void callFunction(int input1, int input2, int index) {
	printFunction(index); //this function will print the table, and set the values of the table array
	int n = input1 * 3 + input2;
	cout << "(" << input1 << "," << input2 << ") => " << table[9 - n] << "\n\n";
}

int* GetTableFromIndex(int tableIndex) {
	int convert = tableIndex;

	for (int i = 9; i > 0; i--) {
		table[i] = convert % 3;
		convert = convert / 3;
	}
	return table;
}



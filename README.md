# MixedRadixCircuitSynthesis

This Electronic Design Automation (EDA) tool allows you to design and verify computer chips. Novel is its ability to design ternary logic chip {0,1,2} or {-1,0,1} in addition to the the regular binary ones {0,1}. The tool automatically converts ternary logic to binary logic when targeting traditional CMOS technology and exports to verilog and HSPICE. By default it uses the Stanford CNTFET library to generate native ternary circuits using variable voltage threshold CNTFET. In combination with Openlane a chip can be designed and transformed to masks that can be send to a foundry. Highly recommended is the [Tinytapeout service](https://tinytapeout.com) which runs every quarter which is both affordable, straightforward to use and has a nice and helpful community on discord.

This work is part of the PhD work of Steven Bos at USN, Norway to make a modern ternary computer. The tool has been presented at IEEE ISCAS 2022, see [paper](https://openarchive.usn.no/usn-xmlui/handle/11250/3051315).  

## Youtube video with some features

[![Youtube link to MRCS tt03-btcalculator project](https://img.youtube.com/vi/-DzVKAxmSQ0/0.jpg)](https://www.youtube.com/watch?v=-DzVKAxmSQ0)

## Offline/Standalone Installation
1. Install Unity Hub
2. Install Unity Version 2023.1.15 beta with WebGL and Visual Studio Community via the Hub 
3. Download this repo (eg. via git clone or download the zip) 
4. In the Unity Hub click open project and go the repo folder
5. The project should be able to run immediately when the play button is pressed in the editor. Alternatively the project can be build as a standalone app for windows or for WebGL. 

## BACKUP
No design or other data is collected/stored online. The online version stores your design in your local cache allowing you to continue between refreshes. Importantly, this means that you are responsible for your own data. The standalone version stores your data in unity [persistent file path](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html). For example in my case the files are stored in C:\Users\[user name]\AppData\LocalLow\USN Kongsberg\MRCS\User.

**Make sure to regularly backup your designs for both the WebGL and standalone version!!** Export your whole library using the *export component* function or simply copy the entire folder. This folder your designs, exports, imports and settings. 

## Online version of MRCS (version 2.1)
* [WebGL version on TernaryResearch.com](https://ternaryresearch.com/mixed-radix-circuit-synthesizer/)

## Projects made with MRCS: 

#### Tiny Tapeout 3
* https://github.com/aiunderstand/tt03-balanced-ternary-calculator

#### Tiny Tapeout 2
* https://github.com/aiunderstand/tt02-async-binary-ternary-convert-compare
* https://github.com/aiunderstand/tt02-4bit-tristate-loadable-counter
* https://github.com/aiunderstand/tt02-mrcs-verilog-test

#### Tiny Tapeout 1
* https://github.com/aiunderstand/tinytapeout_bintristateloadablecounter
* https://github.com/aiunderstand/tinytapeout_asyncbinterconvcomp

## Disclaimer:

This tool comes as is. The author(s) are not liable for any failed chip that might be the result of using this tool. It is still considered early work. The tool focusses a lot on automating verification to reduce the risc of faulty chips. Transistor level verification of the generated netlists are done with HSPICE.  RTL and gate level verification of the generated verilog are done with eg. Vivado. Physical testing of the generated verilog can be done a FPGA (we test with the Digilent Basys 3 Artix-7 FPGA).

Please report any found bugs and feedback using github issues or send an email to: first name.last name@usn.no.

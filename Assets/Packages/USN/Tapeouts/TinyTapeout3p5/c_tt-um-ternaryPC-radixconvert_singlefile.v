`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps
module c_tt_um_ternaryPC_radixconvert (
     input [7:0] ui_in,
     output [7:0] uo_out,
     input  wire [7:0] uio_in,  
     output wire [7:0] uio_out,
     //output wire [7:0] uio_oe, //not needed for FPGA
     //input  wire ena,
     input  wire clk,
     input  wire rst_n
);

wire bnet_0 = clk; //Clock
wire bnet_1 = rst_n; //LoadEn
wire [1:0] tnet_2 = uio_in[7:6]; //Data3
wire [1:0] tnet_3 = ui_in[7:6]; //Data2
wire [1:0] tnet_4 = tnet_3;
wire [1:0] tnet_5 = ui_in[5:4]; //Data1
wire [1:0] tnet_6 = tnet_5;
wire [1:0] tnet_7 = ui_in[3:2]; //Data0
wire [1:0] tnet_8 = tnet_7;
wire [1:0] tnet_9 = ui_in[1:0]; //Dir

wire bnet_10;
wire bnet_11;
wire bnet_12;
wire bnet_13;
wire [1:0] tnet_14;
wire [1:0] tnet_15;
wire [1:0] tnet_16;
wire [1:0] tnet_17;
wire [1:0] tnet_18;
wire [1:0] tnet_19;
wire [1:0] tnet_20;

//assign uio_oe = 8'b00111111; // 0 = input, 1 = output
assign uio_out[7:6] = 2'b00;

assign uo_out[7:6] = tnet_14; //PC3
assign uo_out[5:4] = tnet_15; //PC2
assign uo_out[3:2] = tnet_16; //PC1
assign uo_out[1:0] = tnet_17; //PC0
assign uio_out[5:4] = tnet_18; //RC2
assign uio_out[3:2] = tnet_19; //RC1
assign uio_out[1:0] = tnet_20; //RC0

c_SyTriDirLoadCounter4 SavedGate_0 (
.io_in({bnet_0,bnet_1,tnet_2,tnet_3,tnet_5,tnet_7,tnet_9}),
.io_out({tnet_14,tnet_15,tnet_16,tnet_17})
);

c_SignedBTRadixConverter4 SavedGate_1 (
.io_in({bnet_10,bnet_11,bnet_12,bnet_13}),
.io_out({tnet_18,tnet_18,tnet_19,tnet_20})
);

c_BTSignedRadixConverter4 SavedGate_2 (
.io_in({tnet_4,tnet_6,tnet_8}),
.io_out({bnet_10,bnet_11,bnet_12,bnet_13})
);

endmodule

module c_BHA (
     input [1:0] io_in,
     output [1:0] io_out
);

wire bnet_0 = io_in[1]; //B
wire bnet_1 = bnet_0;
wire bnet_2 = io_in[0]; //A
wire bnet_3 = bnet_2;

wire bnet_4;
wire bnet_5;

assign io_out[1] = bnet_4; //DataOut
assign io_out[0] = bnet_5; //DataOut

f_K00 LogicGate_0 (
.portB(bnet_0),
.portA(bnet_3),
.out(bnet_4)
);

f_20K LogicGate_1 (
.portB(bnet_1),
.portA(bnet_2),
.out(bnet_5)
);

endmodule

module c_BHA3 (
     input [3:0] io_in,
     output [3:0] io_out
);

wire bnet_0 = io_in[3]; //DataIn
wire bnet_1 = io_in[2]; //DataIn
wire bnet_2 = io_in[1]; //DataIn
wire bnet_3 = io_in[0]; //DataIn

wire bnet_4;
wire bnet_5;
wire bnet_6;
wire bnet_7;
wire bnet_8;
wire bnet_9;

assign io_out[3] = bnet_6; //DataOut
assign io_out[2] = bnet_7; //DataOut
assign io_out[1] = bnet_8; //DataOut
assign io_out[0] = bnet_9; //DataOut

c_BHA SavedGate_0 (
.io_in({bnet_1,bnet_4}),
.io_out({bnet_6,bnet_7})
);

c_BHA SavedGate_1 (
.io_in({bnet_2,bnet_5}),
.io_out({bnet_4,bnet_8})
);

c_BHA SavedGate_2 (
.io_in({bnet_0,bnet_3}),
.io_out({bnet_5,bnet_9})
);

endmodule

module c_BTLatch (
     input [2:0] io_in,
     output [1:0] io_out
);

wire bnet_0 = io_in[2]; //DataIn
wire [1:0] tnet_1 = io_in[1:0]; //DataIn

wire [1:0] tnet_2;
wire [1:0] tnet_3 = tnet_2;

assign io_out[1:0] = tnet_3; //DataOut

f_ZD0PPPPPP_bet LogicGate_0 (
.portC({bnet_0,!bnet_0}),
.portA(tnet_1),
.portB(tnet_2),
.out(tnet_2)
);

endmodule

module c_BTSignedRadixConverter4 (
     input [5:0] io_in,
     output [3:0] io_out
);

wire [1:0] tnet_0 = io_in[5:4]; //DataIn
wire [1:0] tnet_1 = tnet_0;
wire [1:0] tnet_2 = tnet_0;
wire [1:0] tnet_3 = io_in[3:2]; //DataIn
wire [1:0] tnet_4 = tnet_3;
wire [1:0] tnet_5 = tnet_3;
wire [1:0] tnet_6 = tnet_3;
wire [1:0] tnet_7 = tnet_3;
wire [1:0] tnet_8 = io_in[1:0]; //DataIn
wire [1:0] tnet_9 = tnet_8;

wire [1:0] tnet_10;
wire [1:0] tnet_11;
wire [1:0] tnet_12 = tnet_11;
wire [1:0] tnet_13 = tnet_11;
wire [1:0] tnet_14;
wire [1:0] tnet_15;
wire [1:0] tnet_16;
wire [1:0] tnet_17;
wire [1:0] tnet_18;

assign io_out[3] = tnet_15[1]; //DataOut
assign io_out[2] = tnet_16[1]; //DataOut
assign io_out[1] = tnet_17[1]; //DataOut
assign io_out[0] = tnet_18[1]; //DataOut

f_228_bet LogicGate_0 (
.portB(tnet_2),
.portA(tnet_10),
.out(tnet_15)
);

f_CC9_bet LogicGate_1 (
.portB(tnet_7),
.portA(tnet_13),
.out(tnet_10)
);

f_N28_bet LogicGate_2 (
.portB(tnet_6),
.portA(tnet_12),
.out(tnet_16)
);

f_EDCRC9DD4_bet LogicGate_3 (
.portC(tnet_1),
.portB(tnet_5),
.portA(tnet_9),
.out(tnet_11)
);

f_2N6_bet LogicGate_4 (
.portB(tnet_4),
.portA(tnet_11),
.out(tnet_17)
);

f_60N_bet LogicGate_5 (
.portB(tnet_0),
.portA(tnet_14),
.out(tnet_18)
);

f_6N6_bet LogicGate_6 (
.portB(tnet_3),
.portA(tnet_8),
.out(tnet_14)
);

endmodule

module c_ConditionalSTI (
     input [2:0] io_in,
     output [1:0] io_out
);

wire bnet_0 = io_in[2]; //Sign
wire [1:0] tnet_1 = io_in[1:0]; //DataIn

wire [1:0] tnet_2;

assign io_out[1:0] = tnet_2; //DataOut

f_5DP_bet LogicGate_0 (
.portB({bnet_0,!bnet_0}),
.portA(tnet_1),
.out(tnet_2)
);

endmodule

module c_ConditionalSTI4 (
     input [8:0] io_in,
     output [7:0] io_out
);

wire bnet_0 = io_in[8]; //Sign
wire bnet_1 = bnet_0;
wire bnet_2 = bnet_0;
wire bnet_3 = bnet_0;
wire [1:0] tnet_4 = io_in[7:6]; //DataIn
wire [1:0] tnet_5 = io_in[5:4]; //DataIn
wire [1:0] tnet_6 = io_in[3:2]; //DataIn
wire [1:0] tnet_7 = io_in[1:0]; //DataIn

//wire [1:0] tnet_8;
wire [1:0] tnet_9;
wire [1:0] tnet_10;
wire [1:0] tnet_11;

//assign io_out[7:6] = tnet_8; //DataOut
assign io_out[5:4] = tnet_9; //DataOut
assign io_out[3:2] = tnet_10; //DataOut
assign io_out[1:0] = tnet_11; //DataOut

//c_ConditionalSTI SavedGate_0 (
//.io_in({bnet_0,tnet_4}),
//.io_out({tnet_8})
//);

c_ConditionalSTI SavedGate_1 (
.io_in({bnet_1,tnet_5}),
.io_out({tnet_9})
);

c_ConditionalSTI SavedGate_2 (
.io_in({bnet_2,tnet_6}),
.io_out({tnet_10})
);

c_ConditionalSTI SavedGate_3 (
.io_in({bnet_3,tnet_7}),
.io_out({tnet_11})
);

endmodule

module c_CONS (
     input [3:0] io_in,
     output [1:0] io_out
);

wire [1:0] tnet_0 = io_in[3:2]; //DataIn
wire [1:0] tnet_1 = io_in[1:0]; //DataIn

wire [1:0] tnet_2;

assign io_out[1:0] = tnet_2; //DataOut

f_RDC_bet LogicGate_0 (
.portB(tnet_0),
.portA(tnet_1),
.out(tnet_2)
);

endmodule

module c_SignedBTRadixConverter4 (
     input [3:0] io_in,
     output [7:0] io_out
);

wire bnet_0 = io_in[3]; //Sign
wire bnet_1 = bnet_0;
wire bnet_2 = bnet_0;
wire bnet_3 = io_in[2]; //DataIn
wire bnet_4 = io_in[1]; //DataIn
wire bnet_5 = io_in[0]; //DataIn

wire [1:0] tnet_6;
wire [1:0] tnet_7;
wire [1:0] tnet_8;
wire [1:0] tnet_9;
wire bnet_10;
wire bnet_11;
wire bnet_12;
wire bnet_13;
wire bnet_14;
wire bnet_15;
wire bnet_16;
wire [1:0] tnet_17;
wire [1:0] tnet_18;
wire [1:0] tnet_19;
wire [1:0] tnet_20;

assign io_out[7:6] = tnet_17; //DataOut
assign io_out[5:4] = tnet_18; //DataOut
assign io_out[3:2] = tnet_19; //DataOut
assign io_out[1:0] = tnet_20; //DataOut

c_ConditionalSTI4 SavedGate_0 (
.io_in({bnet_2,tnet_9,tnet_8,tnet_7,tnet_6}),
.io_out({tnet_17,tnet_18,tnet_19,tnet_20})
);

c_UnsignedBT_RadixConverter4 SavedGate_1 (
.io_in({bnet_10,bnet_11,bnet_12,bnet_13}),
.io_out({tnet_9,tnet_8,tnet_7,tnet_6})
);

c_BHA3 SavedGate_2 (
.io_in({bnet_1,bnet_14,bnet_15,bnet_16}),
.io_out({bnet_10,bnet_11,bnet_12,bnet_13})
);

c_XOR3 SavedGate_3 (
.io_in({bnet_0,bnet_3,bnet_4,bnet_5}),
.io_out({bnet_14,bnet_15,bnet_16})
);

endmodule

module c_SyTriDirLoadCounter (
     input [5:0] io_in,
     output [1:0] io_out
);

wire bnet_0 = io_in[5]; //Clock
wire bnet_1 = io_in[4]; //LoadEn
wire [1:0] tnet_2 = io_in[3:2]; //Data
wire [1:0] tnet_3 = io_in[1:0]; //Dir

wire [1:0] tnet_4;
wire [1:0] tnet_5;
wire [1:0] tnet_6;
wire [1:0] tnet_7 = tnet_6;

assign io_out[1:0] = tnet_7; //DataOut

f_PPPPPPZD0_bet LogicGate_0 (
.portC({bnet_1,!bnet_1}),
.portB(tnet_2),
.portA(tnet_5),
.out(tnet_4)
);

f_7PB_bet LogicGate_1 (
.portB(tnet_3),
.portA(tnet_6),
.out(tnet_5)
);

c_TFF SavedGate_0 (
.io_in({bnet_0,tnet_4}),
.io_out({tnet_6})
);

endmodule

module c_SyTriDirLoadCounter4 (
     input [11:0] io_in,
     output [7:0] io_out
);

wire bnet_0 = io_in[11]; //Clock
wire bnet_1 = bnet_0;
wire bnet_2 = bnet_0;
wire bnet_3 = bnet_0;
wire bnet_4 = io_in[10]; //LoadEn
wire bnet_5 = bnet_4;
wire bnet_6 = bnet_4;
wire bnet_7 = bnet_4;
wire [1:0] tnet_8 = io_in[9:8]; //Data3
wire [1:0] tnet_9 = io_in[7:6]; //Data2
wire [1:0] tnet_10 = io_in[5:4]; //Data1
wire [1:0] tnet_11 = io_in[3:2]; //Data0
wire [1:0] tnet_12 = io_in[1:0]; //Dir
wire [1:0] tnet_13 = tnet_12;

wire [1:0] tnet_14;
wire [1:0] tnet_15;
wire [1:0] tnet_16;
wire [1:0] tnet_17 = tnet_16;
wire [1:0] tnet_18;
wire [1:0] tnet_19;
wire [1:0] tnet_20 = tnet_19;
wire [1:0] tnet_21;
wire [1:0] tnet_22;
wire [1:0] tnet_23 = tnet_15;
wire [1:0] tnet_24 = tnet_18;
wire [1:0] tnet_25 = tnet_21;

assign io_out[7:6] = tnet_22; //Data3
assign io_out[5:4] = tnet_23; //Data2
assign io_out[3:2] = tnet_24; //Data1
assign io_out[1:0] = tnet_25; //Data0

c_SyTriDirLoadCounter SavedGate_0 (
.io_in({bnet_3,bnet_7,tnet_8,tnet_14}),
.io_out({tnet_22})
);

c_CONS SavedGate_1 (
.io_in({tnet_15,tnet_17}),
.io_out({tnet_14})
);

c_SyTriDirLoadCounter SavedGate_2 (
.io_in({bnet_2,bnet_6,tnet_9,tnet_16}),
.io_out({tnet_15})
);

c_CONS SavedGate_3 (
.io_in({tnet_18,tnet_20}),
.io_out({tnet_16})
);

c_SyTriDirLoadCounter SavedGate_4 (
.io_in({bnet_1,bnet_5,tnet_10,tnet_19}),
.io_out({tnet_18})
);

c_CONS SavedGate_5 (
.io_in({tnet_21,tnet_12}),
.io_out({tnet_19})
);

c_SyTriDirLoadCounter SavedGate_6 (
.io_in({bnet_0,bnet_4,tnet_11,tnet_13}),
.io_out({tnet_21})
);

endmodule

module c_TFF (
     input [2:0] io_in,
     output [1:0] io_out
);

wire bnet_0 = io_in[2]; //DataIn
wire bnet_1 = bnet_0;
wire [1:0] tnet_2 = io_in[1:0]; //DataIn

wire bnet_3;
wire [1:0] tnet_4;
wire [1:0] tnet_5;

assign io_out[1:0] = tnet_5; //DataOut

f_2 LogicGate_0 (
.portA(bnet_0),
.out(bnet_3)
);

c_BTLatch SavedGate_0 (
.io_in({bnet_1,tnet_4}),
.io_out({tnet_5})
);

c_BTLatch SavedGate_1 (
.io_in({bnet_3,tnet_2}),
.io_out({tnet_4})
);

endmodule

module c_UnsignedBT_RadixConverter4 (
     input [3:0] io_in,
     output [7:0] io_out
);

wire bnet_0 = io_in[3]; //b3
wire bnet_1 = bnet_0;
wire bnet_2 = bnet_0;
wire bnet_3 = bnet_0;
wire bnet_4 = io_in[2]; //b2
wire bnet_5 = bnet_4;
wire bnet_6 = bnet_4;
wire bnet_7 = bnet_4;
wire bnet_8 = io_in[1]; //b1
wire bnet_9 = bnet_8;
wire bnet_10 = bnet_8;
wire bnet_11 = io_in[0]; //b0
wire bnet_12 = bnet_11;

wire [1:0] tnet_13;
wire [1:0] tnet_14 = tnet_13;
wire [1:0] tnet_15;
wire [1:0] tnet_16 = tnet_15;
wire [1:0] tnet_17;
wire [1:0] tnet_18 = tnet_17;
wire [1:0] tnet_19;
wire [1:0] tnet_20;
wire [1:0] tnet_21;
wire [1:0] tnet_22 = tnet_21;
wire [1:0] tnet_23;
wire [1:0] tnet_24;
wire [1:0] tnet_25;
wire [1:0] tnet_26;
wire [1:0] tnet_27;

assign io_out[7:6] = tnet_24; //t3
assign io_out[5:4] = tnet_25; //t2
assign io_out[3:2] = tnet_26; //t1
assign io_out[1:0] = tnet_27; //t0

f_RRD_bet LogicGate_0 (
.portA({bnet_3,!bnet_3}),
.portB(tnet_14),
.out(tnet_24)
);

f_RRDRDDDDD_bet LogicGate_1 (
.portB({bnet_7,!bnet_7}),
.portA(tnet_16),
.portC(tnet_18),
.out(tnet_13)
);

f_88R_bet LogicGate_2 (
.portA({bnet_2,!bnet_2}),
.portB(tnet_13),
.out(tnet_25)
);

f_ZZR_bet LogicGate_3 (
.portA({bnet_10,!bnet_10}),
.portB(tnet_19),
.out(tnet_15)
);

f_DD4_bet LogicGate_4 (
.portB({bnet_1,!bnet_1}),
.portA(tnet_22),
.out(tnet_17)
);

f_HHDDXXDDD_bet LogicGate_5 (
.portC({bnet_5,!bnet_5}),
.portB({bnet_9,!bnet_9}),
.portA({bnet_12,!bnet_12}),
.out(tnet_19)
);

f_XE2_bet LogicGate_6 (
.portB(tnet_17),
.portA(tnet_20),
.out(tnet_26)
);

f_5XX_bet LogicGate_7 (
.portB({bnet_6,!bnet_6}),
.portA(tnet_15),
.out(tnet_20)
);

f_H4K_bet LogicGate_8 (
.portB({bnet_0,!bnet_0}),
.portA(tnet_21),
.out(tnet_27)
);

f_5XC_bet LogicGate_9 (
.portB({bnet_4,!bnet_4}),
.portA(tnet_23),
.out(tnet_21)
);

f_HE4_bet LogicGate_10 (
.portB({bnet_8,!bnet_8}),
.portA({bnet_11,!bnet_11}),
.out(tnet_23)
);

endmodule

module c_XOR (
     input [1:0] io_in,
     output [0:0] io_out
);

wire bnet_0 = io_in[1]; //DataIn
wire bnet_1 = io_in[0]; //DataIn

wire bnet_2;

assign io_out[0] = bnet_2; //DataOut

f_20K LogicGate_0 (
.portB(bnet_0),
.portA(bnet_1),
.out(bnet_2)
);

endmodule

module c_XOR3 (
     input [3:0] io_in,
     output [2:0] io_out
);

wire bnet_0 = io_in[3]; //Sign
wire bnet_1 = bnet_0;
wire bnet_2 = bnet_0;
wire bnet_3 = io_in[2]; //DataIn
wire bnet_4 = io_in[1]; //DataIn
wire bnet_5 = io_in[0]; //DataIn

wire bnet_6;
wire bnet_7;
wire bnet_8;

assign io_out[2] = bnet_6; //DataOut
assign io_out[1] = bnet_7; //DataOut
assign io_out[0] = bnet_8; //DataOut

c_XOR SavedGate_0 (
.io_in({bnet_0,bnet_3}),
.io_out({bnet_6})
);

c_XOR SavedGate_1 (
.io_in({bnet_1,bnet_4}),
.io_out({bnet_7})
);

c_XOR SavedGate_2 (
.io_in({bnet_2,bnet_5}),
.io_out({bnet_8})
);

endmodule

module f_2 (
     input wire portA,
     output wire out
     );

     assign out = 
    (portA == 0);
endmodule

module f_20K (
     input wire portB,
     input wire portA,
     output wire out
     );

     assign out = 
    (portB == 1 & portA == 0) |
    (portB == 0 & portA == 1);
endmodule

module f_228_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b01) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b11) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b11) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b10) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b01 :
     2'b11;
endmodule

module f_2N6_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b11) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b11) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b10) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b01 :
     2'b11;
endmodule

module f_5DP_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b01 :
     2'b11;
endmodule

module f_5XC_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b11) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b01 :
     2'b11;
endmodule

module f_5XX_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b11) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b01 :
     2'b11;
endmodule

module f_60N_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b01) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b01) & (portA == 2'b11) ? 2'b01 :
    (portB == 2'b11) & (portA == 2'b11) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b11) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b10) ? 2'b01 :
    (portB == 2'b11) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b01 :
     2'b11;
endmodule

module f_6N6_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b11) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b11) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b01) & (portA == 2'b10) ? 2'b01 :
    (portB == 2'b11) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b01 :
     2'b11;
endmodule

module f_7PB_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b01) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b11) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b01 :
     2'b11;
endmodule

module f_88R_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b10) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b01) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b11) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b01 :
     2'b11;
endmodule

module f_CC9_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b11) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b11) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b10) ? 2'b01 :
     2'b11;
endmodule

module f_DD4_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b10) & (portA == 2'b01) ? 2'b01 :
     2'b11;
endmodule

module f_EDCRC9DD4_bet (
     input wire[1:0] portC,
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portC == 2'b01) & (portB == 2'b10) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b11) & (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b11) & (portB == 2'b11) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b11) & (portB == 2'b01) & (portA == 2'b11) ? 2'b01 :
    (portC == 2'b11) & (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
     2'b11;
endmodule

module f_H4K_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b11) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b11) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b10) ? 2'b10 :
     2'b11;
endmodule

module f_HE4_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b10) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b10) ? 2'b10 :
     2'b11;
endmodule

module f_HHDDXXDDD_bet (
     input wire[1:0] portC,
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portC == 2'b11) & (portB == 2'b11) & (portA == 2'b01) ? 2'b10 :
    (portC == 2'b11) & (portB == 2'b10) & (portA == 2'b01) ? 2'b10 :
    (portC == 2'b11) & (portB == 2'b11) & (portA == 2'b11) ? 2'b10 :
    (portC == 2'b11) & (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b11) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b11) & (portA == 2'b11) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b11) & (portA == 2'b10) ? 2'b10 :
     2'b11;
endmodule

module f_K00 (
     input wire portB,
     input wire portA,
     output wire out
     );

     assign out = 
    (portB == 1 & portA == 1);
endmodule

module f_N28_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b01) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b11) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b11) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b10) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
     2'b11;
endmodule

module f_PPPPPPZD0_bet (
     input wire[1:0] portC,
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portC == 2'b01) & (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b01) & (portB == 2'b11) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b01) & (portB == 2'b10) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b01) & (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b01) & (portB == 2'b11) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b01) & (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b11) & (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b11) & (portB == 2'b10) & (portA == 2'b01) ? 2'b10 :
    (portC == 2'b11) & (portB == 2'b01) & (portA == 2'b11) ? 2'b01 :
    (portC == 2'b11) & (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portC == 2'b11) & (portB == 2'b01) & (portA == 2'b10) ? 2'b01 :
    (portC == 2'b11) & (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b10) & (portB == 2'b10) & (portA == 2'b01) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b11) ? 2'b01 :
    (portC == 2'b10) & (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b10) ? 2'b01 :
    (portC == 2'b10) & (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
     2'b11;
endmodule

module f_RDC_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
     2'b11;
endmodule

module f_RRDRDDDDD_bet (
     input wire[1:0] portC,
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portC == 2'b11) & (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
     2'b11;
endmodule

module f_RRD_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
     2'b11;
endmodule

module f_XE2_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b01) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
     2'b11;
endmodule

module f_ZD0PPPPPP_bet (
     input wire[1:0] portC,
     input wire[1:0] portB,
     input wire[1:0] portA,
     output reg[1:0] out
     );

     always @(posedge portC[1])
     out <= 
     (portA == 2'b01) ? 2'b01 :
     (portA == 2'b10) ? 2'b10 :
      2'b11 ;
endmodule

module f_ZZR_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b10) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b01) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b11) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
     2'b11;
endmodule
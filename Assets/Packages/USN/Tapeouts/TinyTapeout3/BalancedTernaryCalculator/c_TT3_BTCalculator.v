module c_TT3_BTCalculator (
     input [7:0] io_in,
     output [7:0] io_out
);

wire [1:0] tnet_0 = io_in[7:6]; //x1
wire [1:0] tnet_1 = tnet_0;
wire [1:0] tnet_2 = tnet_0;
wire [1:0] tnet_3 = io_in[5:4]; //x0
wire [1:0] tnet_4 = tnet_3;
wire [1:0] tnet_5 = io_in[3:2]; //y1
wire [1:0] tnet_6 = tnet_5;
wire [1:0] tnet_7 = io_in[1:0]; //y0
wire [1:0] tnet_8 = tnet_7;

wire [1:0] tnet_9;
wire [1:0] tnet_10;
wire [1:0] tnet_11;
wire [1:0] tnet_12;
wire [1:0] tnet_13;
wire [1:0] tnet_14;
wire [1:0] tnet_15;
wire [1:0] tnet_16;
wire [1:0] tnet_17;
wire [1:0] tnet_18;
wire [1:0] tnet_19;
wire [1:0] tnet_20;

assign io_out[7:6] = tnet_17; //s3
assign io_out[5:4] = tnet_18; //s2
assign io_out[3:2] = tnet_19; //s1
assign io_out[1:0] = tnet_20; //s0

c_BTM4 SavedGate_0 (
.io_in({tnet_0,tnet_3,tnet_5,tnet_7}),
.io_out({tnet_9,tnet_10,tnet_11,tnet_12})
);

c_Deselect4 SavedGate_1 (
.io_in({tnet_2[1],tnet_9,tnet_10,tnet_11,tnet_12,tnet_13,tnet_14,tnet_15,tnet_16}),
.io_out({tnet_17,tnet_18,tnet_19,tnet_20})
);

c_BTA4 SavedGate_2 (
.io_in({tnet_1,tnet_4,tnet_6,tnet_8}),
.io_out({tnet_13,tnet_14,tnet_15,tnet_16})
);

endmodule

module c_BTA (
     input [3:0] io_in,
     output [3:0] io_out
);

wire [1:0] tnet_0 = io_in[3:2]; //x
wire [1:0] tnet_1 = tnet_0;
wire [1:0] tnet_2 = io_in[1:0]; //y
wire [1:0] tnet_3 = tnet_2;

wire [1:0] tnet_4;
wire [1:0] tnet_5;

assign io_out[3:2] = tnet_4; //s1
assign io_out[1:0] = tnet_5; //s0

f_RDC_bet LogicGate_0 (
.portB(tnet_0),
.portA(tnet_3),
.out(tnet_4)
);

f_7PB_bet LogicGate_1 (
.portB(tnet_1),
.portA(tnet_2),
.out(tnet_5)
);

endmodule

module c_BTA4 (
     input [7:0] io_in,
     output [7:0] io_out
);

wire [1:0] tnet_0 = io_in[7:6]; //x1
wire [1:0] tnet_1 = io_in[5:4]; //x0
wire [1:0] tnet_2 = io_in[3:2]; //y1
wire [1:0] tnet_3 = io_in[1:0]; //y0

wire [1:0] tnet_4;
wire [1:0] tnet_5;
wire [1:0] tnet_6;
wire [1:0] tnet_7;
wire [1:0] tnet_8;
wire [1:0] tnet_9;
wire [1:0] tnet_10;
wire [1:0] tnet_11;

assign io_out[7:6] = tnet_8; //s3
assign io_out[5:4] = tnet_9; //s2
assign io_out[3:2] = tnet_10; //s1
assign io_out[1:0] = tnet_11; //s0

c_BTA SavedGate_0 (
.io_in({tnet_5,tnet_6}),
.io_out({tnet_8,tnet_9})
);

c_BTA SavedGate_1 (
.io_in({tnet_0,tnet_2}),
.io_out({tnet_5,tnet_4})
);

c_BTA SavedGate_2 (
.io_in({tnet_4,tnet_7}),
.io_out({tnet_6,tnet_10})
);

c_BTA SavedGate_3 (
.io_in({tnet_1,tnet_3}),
.io_out({tnet_7,tnet_11})
);

endmodule

module c_BTM (
     input [3:0] io_in,
     output [1:0] io_out
);

wire [1:0] tnet_0 = io_in[3:2]; //x
wire [1:0] tnet_1 = io_in[1:0]; //y

wire [1:0] tnet_2;

assign io_out[1:0] = tnet_2; //out

f_PD5_bet LogicGate_0 (
.portB(tnet_0),
.portA(tnet_1),
.out(tnet_2)
);

endmodule

module c_BTM4 (
     input [7:0] io_in,
     output [7:0] io_out
);

wire [1:0] tnet_0 = io_in[7:6]; //x1
wire [1:0] tnet_1 = tnet_0;
wire [1:0] tnet_2 = io_in[5:4]; //x0
wire [1:0] tnet_3 = tnet_2;
wire [1:0] tnet_4 = io_in[3:2]; //y1
wire [1:0] tnet_5 = tnet_4;
wire [1:0] tnet_6 = io_in[1:0]; //y0
wire [1:0] tnet_7 = tnet_6;

wire [1:0] tnet_8;
wire [1:0] tnet_9;
wire [1:0] tnet_10;
wire [1:0] tnet_11;
wire [1:0] tnet_12 = tnet_11;
wire [1:0] tnet_13;
wire [1:0] tnet_14;
wire [1:0] tnet_15 = tnet_14;
wire [1:0] tnet_16;
wire [1:0] tnet_17 = tnet_8;
wire [1:0] tnet_18 = tnet_11;
wire [1:0] tnet_19 = tnet_14;

assign io_out[7:6] = tnet_16; //s3
assign io_out[5:4] = tnet_17; //s2
assign io_out[3:2] = tnet_18; //s1
assign io_out[1:0] = tnet_19; //s0

f_DD4DDDEDD_bet LogicGate_0 (
.portC(tnet_8),
.portB(tnet_12),
.portA(tnet_15),
.out(tnet_16)
);

f_CZGDDDA0R_bet LogicGate_1 (
.portC(tnet_9),
.portB(tnet_11),
.portA(tnet_14),
.out(tnet_8)
);

c_BTM SavedGate_0 (
.io_in({tnet_0,tnet_4}),
.io_out({tnet_9})
);

c_BTM SavedGate_1 (
.io_in({tnet_1,tnet_7}),
.io_out({tnet_10})
);

c_SUM SavedGate_2 (
.io_in({tnet_10,tnet_13}),
.io_out({tnet_11})
);

c_BTM SavedGate_3 (
.io_in({tnet_3,tnet_5}),
.io_out({tnet_13})
);

c_BTM SavedGate_4 (
.io_in({tnet_2,tnet_6}),
.io_out({tnet_14})
);

endmodule

module c_Deselect4 (
     input [16:0] io_in,
     output [7:0] io_out
);

wire bnet_0 = io_in[16]; //select
wire bnet_1 = bnet_0;
wire bnet_2 = bnet_0;
wire bnet_3 = bnet_0;
wire bnet_4 = bnet_0;
wire [1:0] tnet_5 = io_in[15:14]; //a3
wire [1:0] tnet_6 = io_in[13:12]; //a2
wire [1:0] tnet_7 = io_in[11:10]; //a1
wire [1:0] tnet_8 = io_in[9:8]; //a0
wire [1:0] tnet_9 = io_in[7:6]; //b3
wire [1:0] tnet_10 = io_in[5:4]; //b2
wire [1:0] tnet_11 = io_in[3:2]; //b1
wire [1:0] tnet_12 = io_in[1:0]; //b0

wire [1:0] tnet_13;
wire [1:0] tnet_14;
wire [1:0] tnet_15;
wire [1:0] tnet_16;
wire [1:0] tnet_17;
wire [1:0] tnet_18;
wire [1:0] tnet_19;
wire [1:0] tnet_20;
wire bnet_21;
wire bnet_22 = bnet_21;
wire bnet_23 = bnet_21;
wire bnet_24 = bnet_21;
wire [1:0] tnet_25;
wire [1:0] tnet_26;
wire [1:0] tnet_27;
wire [1:0] tnet_28;

assign io_out[7:6] = tnet_25; //s3
assign io_out[5:4] = tnet_26; //s2
assign io_out[3:2] = tnet_27; //s1
assign io_out[1:0] = tnet_28; //s0

f_RD4_bet LogicGate_0 (
.portA(tnet_5),
.portB({bnet_21,!bnet_21}),
.out(tnet_13)
);

f_RD4_bet LogicGate_1 (
.portA(tnet_6),
.portB({bnet_22,!bnet_22}),
.out(tnet_14)
);

f_RD4_bet LogicGate_2 (
.portA(tnet_7),
.portB({bnet_23,!bnet_23}),
.out(tnet_15)
);

f_RD4_bet LogicGate_3 (
.portA(tnet_8),
.portB({bnet_24,!bnet_24}),
.out(tnet_16)
);

f_VP0_bet LogicGate_4 (
.portB(tnet_13),
.portA(tnet_17),
.out(tnet_25)
);

f_VP0_bet LogicGate_5 (
.portB(tnet_14),
.portA(tnet_18),
.out(tnet_26)
);

f_VP0_bet LogicGate_6 (
.portB(tnet_15),
.portA(tnet_19),
.out(tnet_27)
);

f_VP0_bet LogicGate_7 (
.portB(tnet_16),
.portA(tnet_20),
.out(tnet_28)
);

f_RD4_bet LogicGate_8 (
.portB({bnet_1,!bnet_1}),
.portA(tnet_9),
.out(tnet_17)
);

f_RD4_bet LogicGate_9 (
.portB({bnet_2,!bnet_2}),
.portA(tnet_10),
.out(tnet_18)
);

f_RD4_bet LogicGate_10 (
.portB({bnet_3,!bnet_3}),
.portA(tnet_11),
.out(tnet_19)
);

f_RD4_bet LogicGate_11 (
.portB({bnet_4,!bnet_4}),
.portA(tnet_12),
.out(tnet_20)
);

c_NOT SavedGate_0 (
.io_in({bnet_0}),
.io_out({bnet_21})
);

endmodule

module c_NOT (
     input [0:0] io_in,
     output [0:0] io_out
);

wire bnet_0 = io_in[0]; //Select

wire bnet_1;

assign io_out[0] = bnet_1; //DataOut

f_2 LogicGate_0 (
.portA(bnet_0),
.out(bnet_1)
);

endmodule

module c_SUM (
     input [3:0] io_in,
     output [1:0] io_out
);

wire [1:0] tnet_0 = io_in[3:2]; //x
wire [1:0] tnet_1 = io_in[1:0]; //y

wire [1:0] tnet_2;

assign io_out[1:0] = tnet_2; //out

f_7PB_bet LogicGate_0 (
.portB(tnet_0),
.portA(tnet_1),
.out(tnet_2)
);

endmodule

module f_2 (
     input wire portA,
     output wire out
     );

     assign out = 
    (portA == 0);
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

module f_CZGDDDA0R_bet (
     input wire[1:0] portC,
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portC == 2'b01) & (portB == 2'b10) & (portA == 2'b01) ? 2'b10 :
    (portC == 2'b01) & (portB == 2'b01) & (portA == 2'b11) ? 2'b01 :
    (portC == 2'b01) & (portB == 2'b11) & (portA == 2'b11) ? 2'b01 :
    (portC == 2'b01) & (portB == 2'b10) & (portA == 2'b11) ? 2'b01 :
    (portC == 2'b01) & (portB == 2'b11) & (portA == 2'b10) ? 2'b01 :
    (portC == 2'b10) & (portB == 2'b11) & (portA == 2'b01) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b11) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b11) & (portA == 2'b11) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b10) ? 2'b01 :
     2'b11;
endmodule

module f_DD4DDDEDD_bet (
     input wire[1:0] portC,
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portC == 2'b01) & (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b10) & (portA == 2'b01) ? 2'b01 :
     2'b11;
endmodule

module f_PD5_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b01) & (portA == 2'b01) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b10) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
     2'b11;
endmodule

module f_RD4_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b10) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
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

module f_VP0_bet (
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b11) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b01) ? 2'b01 :
    (portB == 2'b01) & (portA == 2'b11) ? 2'b01 :
    (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portB == 2'b01) & (portA == 2'b10) ? 2'b01 :
    (portB == 2'b11) & (portA == 2'b10) ? 2'b10 :
    (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
     2'b11;
endmodule


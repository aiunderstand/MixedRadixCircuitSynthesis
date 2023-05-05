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

module f_2 (
     input wire portA,
     output wire out
     );

     assign out = 
    (portA == 0);
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


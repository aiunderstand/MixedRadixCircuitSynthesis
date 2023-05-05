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


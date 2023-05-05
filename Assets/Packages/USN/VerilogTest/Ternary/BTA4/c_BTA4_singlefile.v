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


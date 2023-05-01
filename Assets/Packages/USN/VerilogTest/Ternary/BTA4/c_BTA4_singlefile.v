module c_BTA4 (
     input [7:0] io_in,
     output [7:0] io_out
);

wire [1:0] tnet_0 = io_in[1:0];
wire [1:0] tnet_1 = io_in[3:2];
wire [1:0] tnet_2 = io_in[5:4];
wire [1:0] tnet_3 = io_in[7:6];

wire [1:0] tnet_4;
wire [1:0] tnet_5;
wire [1:0] tnet_6;
wire [1:0] tnet_7;
wire [1:0] tnet_8;
wire [1:0] tnet_9;
wire [1:0] tnet_10;
wire [1:0] tnet_11;

assign io_out[1:0] = tnet_8;
assign io_out[3:2] = tnet_9;
assign io_out[5:4] = tnet_10;
assign io_out[7:6] = tnet_11;

c_BTA SavedGate_0 (
.io_in({tnet_3,tnet_1}),
.io_out({tnet_11,tnet_4})
);

c_BTA SavedGate_1 (
.io_in({tnet_2,tnet_0}),
.io_out({tnet_5,tnet_6})
);

c_BTA SavedGate_2 (
.io_in({tnet_4,tnet_5}),
.io_out({tnet_10,tnet_7})
);

c_BTA SavedGate_3 (
.io_in({tnet_7,tnet_6}),
.io_out({tnet_9,tnet_8})
);

endmodule

module c_BTA (
     input [3:0] io_in,
     output [3:0] io_out
);

wire [1:0] tnet_0 = io_in[1:0];
wire [1:0] tnet_1 = tnet_0;
wire [1:0] tnet_2 = io_in[3:2];
wire [1:0] tnet_3 = tnet_2;

wire [1:0] tnet_4;
wire [1:0] tnet_5;

assign io_out[1:0] = tnet_4;
assign io_out[3:2] = tnet_5;

f_RDC_bet LogicGate_0 (
.in_1(tnet_0),
.in_0(tnet_3),
.out_0(tnet_4)
);

f_7PB_bet LogicGate_1 (
.in_1(tnet_1),
.in_0(tnet_2),
.out_0(tnet_5)
);

endmodule

module f_7PB_bet (
     input wire[1:0] in_0,
     input wire[1:0] in_1,
     output wire[1:0] out_0
     );

     assign out_0 = 
(in_0 == 2'b01) & (in_1 == 2'b01) ? 2'b10 :
(in_0 == 2'b01) & (in_1 == 2'b11) ? 2'b01 :
(in_0 == 2'b01) & (in_1 == 2'b10) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b01) ? 2'b01 :
(in_0 == 2'b11) & (in_1 == 2'b11) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b10) ? 2'b10 :
(in_0 == 2'b10) & (in_1 == 2'b01) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b11) ? 2'b10 :
(in_0 == 2'b10) & (in_1 == 2'b10) ? 2'b01 :
2'b00;
endmodule

module f_RDC_bet (
     input wire[1:0] in_0,
     input wire[1:0] in_1,
     output wire[1:0] out_0
     );

     assign out_0 = 
(in_0 == 2'b01) & (in_1 == 2'b01) ? 2'b01 :
(in_0 == 2'b01) & (in_1 == 2'b11) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b10) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b01) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b11) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b10) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b01) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b11) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b10) ? 2'b10 :
2'b00;
endmodule


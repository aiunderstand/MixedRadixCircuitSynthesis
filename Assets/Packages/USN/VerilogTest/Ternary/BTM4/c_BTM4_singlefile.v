module c_BTM4raw (
     input [7:0] io_in,
     output [7:0] io_out
);

wire [1:0] tnet_0 = io_in[1:0];
wire [1:0] tnet_1 = tnet_0;
wire [1:0] tnet_2 = io_in[3:2];
wire [1:0] tnet_3 = tnet_2;
wire [1:0] tnet_4 = io_in[5:4];
wire [1:0] tnet_5 = tnet_4;
wire [1:0] tnet_6 = io_in[7:6];
wire [1:0] tnet_7 = tnet_6;

wire [1:0] tnet_8;
wire [1:0] tnet_9 = tnet_8;
wire [1:0] tnet_10;
wire [1:0] tnet_11;
wire [1:0] tnet_12;
wire [1:0] tnet_13;
wire [1:0] tnet_14 = tnet_13;
wire [1:0] tnet_15;
wire [1:0] tnet_16;
wire [1:0] tnet_17 = tnet_15;
wire [1:0] tnet_18 = tnet_13;
wire [1:0] tnet_19 = tnet_8;

assign io_out[1:0] = tnet_16;
assign io_out[3:2] = tnet_17;
assign io_out[5:4] = tnet_18;
assign io_out[7:6] = tnet_19;

f_PD5_bet LogicGate_0 (
.in_1(tnet_3),
.in_0(tnet_7),
.out_0(tnet_8)
);

f_PD5_bet LogicGate_1 (
.in_1(tnet_2),
.in_0(tnet_5),
.out_0(tnet_10)
);

f_PD5_bet LogicGate_2 (
.in_1(tnet_1),
.in_0(tnet_6),
.out_0(tnet_11)
);

f_PD5_bet LogicGate_3 (
.in_1(tnet_0),
.in_0(tnet_4),
.out_0(tnet_12)
);

f_7PB_bet LogicGate_4 (
.in_1(tnet_11),
.in_0(tnet_10),
.out_0(tnet_13)
);

f_CZGDDDA0R_bet LogicGate_5 (
.in_2(tnet_12),
.in_1(tnet_13),
.in_0(tnet_8),
.out_0(tnet_15)
);

f_DD4DDDEDD_bet LogicGate_6 (
.in_2(tnet_15),
.in_1(tnet_14),
.in_0(tnet_9),
.out_0(tnet_16)
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

module f_CZGDDDA0R_bet (
     input wire[1:0] in_0,
     input wire[1:0] in_1,
     input wire[1:0] in_2,
     output wire[1:0] out_0
     );

     assign out_0 = 
(in_0 == 2'b01) & (in_1 == 2'b01) & (in_2 == 2'b01) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b11) & (in_2 == 2'b01) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b10) & (in_2 == 2'b01) ? 2'b10 :
(in_0 == 2'b11) & (in_1 == 2'b01) & (in_2 == 2'b01) ? 2'b01 :
(in_0 == 2'b11) & (in_1 == 2'b11) & (in_2 == 2'b01) ? 2'b01 :
(in_0 == 2'b11) & (in_1 == 2'b10) & (in_2 == 2'b01) ? 2'b01 :
(in_0 == 2'b10) & (in_1 == 2'b01) & (in_2 == 2'b01) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b11) & (in_2 == 2'b01) ? 2'b01 :
(in_0 == 2'b10) & (in_1 == 2'b10) & (in_2 == 2'b01) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b01) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b11) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b10) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b01) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b11) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b10) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b01) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b11) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b10) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b01) & (in_2 == 2'b10) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b11) & (in_2 == 2'b10) ? 2'b10 :
(in_0 == 2'b01) & (in_1 == 2'b10) & (in_2 == 2'b10) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b01) & (in_2 == 2'b10) ? 2'b10 :
(in_0 == 2'b11) & (in_1 == 2'b11) & (in_2 == 2'b10) ? 2'b10 :
(in_0 == 2'b11) & (in_1 == 2'b10) & (in_2 == 2'b10) ? 2'b10 :
(in_0 == 2'b10) & (in_1 == 2'b01) & (in_2 == 2'b10) ? 2'b01 :
(in_0 == 2'b10) & (in_1 == 2'b11) & (in_2 == 2'b10) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b10) & (in_2 == 2'b10) ? 2'b11 :
2'b00;
endmodule

module f_DD4DDDEDD_bet (
     input wire[1:0] in_0,
     input wire[1:0] in_1,
     input wire[1:0] in_2,
     output wire[1:0] out_0
     );

     assign out_0 = 
(in_0 == 2'b01) & (in_1 == 2'b01) & (in_2 == 2'b01) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b11) & (in_2 == 2'b01) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b10) & (in_2 == 2'b01) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b01) & (in_2 == 2'b01) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b11) & (in_2 == 2'b01) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b10) & (in_2 == 2'b01) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b01) & (in_2 == 2'b01) ? 2'b10 :
(in_0 == 2'b10) & (in_1 == 2'b11) & (in_2 == 2'b01) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b10) & (in_2 == 2'b01) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b01) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b11) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b10) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b01) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b11) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b10) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b01) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b11) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b10) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b01) & (in_2 == 2'b10) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b11) & (in_2 == 2'b10) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b10) & (in_2 == 2'b10) ? 2'b01 :
(in_0 == 2'b11) & (in_1 == 2'b01) & (in_2 == 2'b10) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b11) & (in_2 == 2'b10) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b10) & (in_2 == 2'b10) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b01) & (in_2 == 2'b10) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b11) & (in_2 == 2'b10) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b10) & (in_2 == 2'b10) ? 2'b11 :
2'b00;
endmodule

module f_PD5_bet (
     input wire[1:0] in_0,
     input wire[1:0] in_1,
     output wire[1:0] out_0
     );

     assign out_0 = 
(in_0 == 2'b01) & (in_1 == 2'b01) ? 2'b10 :
(in_0 == 2'b01) & (in_1 == 2'b11) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b10) ? 2'b01 :
(in_0 == 2'b11) & (in_1 == 2'b01) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b11) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b10) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b01) ? 2'b01 :
(in_0 == 2'b10) & (in_1 == 2'b11) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b10) ? 2'b10 :
2'b00;
endmodule


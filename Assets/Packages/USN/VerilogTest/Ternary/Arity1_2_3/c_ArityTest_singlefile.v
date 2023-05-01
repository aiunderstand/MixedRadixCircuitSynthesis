module c_Arity123Test (
     input [5:0] io_in,
     output [5:0] io_out
);

wire [1:0] tnet_0 = io_in[1:0];
wire [1:0] tnet_1 = io_in[3:2];
wire [1:0] tnet_2 = tnet_1;
wire [1:0] tnet_3 = io_in[5:4];
wire [1:0] tnet_4 = tnet_3;
wire [1:0] tnet_5 = tnet_3;

wire [1:0] tnet_6;
wire [1:0] tnet_7;
wire [1:0] tnet_8;

assign io_out[1:0] = tnet_6;
assign io_out[3:2] = tnet_7;
assign io_out[5:4] = tnet_8;

f_045ZRPDDD_bet LogicGate_0 (
.in_2(tnet_0),
.in_1(tnet_1),
.in_0(tnet_3),
.out_0(tnet_6)
);

f_7AR_bet LogicGate_1 (
.in_1(tnet_2),
.in_0(tnet_4),
.out_0(tnet_7)
);

f_5_bet LogicGate_2 (
.in_0(tnet_5),
.out_0(tnet_8)
);

endmodule

module f_045ZRPDDD_bet (
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
(in_0 == 2'b10) & (in_1 == 2'b01) & (in_2 == 2'b01) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b11) & (in_2 == 2'b01) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b10) & (in_2 == 2'b01) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b01) & (in_2 == 2'b11) ? 2'b01 :
(in_0 == 2'b01) & (in_1 == 2'b11) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b10) & (in_2 == 2'b11) ? 2'b10 :
(in_0 == 2'b11) & (in_1 == 2'b01) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b11) & (in_2 == 2'b11) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b10) & (in_2 == 2'b11) ? 2'b10 :
(in_0 == 2'b10) & (in_1 == 2'b01) & (in_2 == 2'b11) ? 2'b10 :
(in_0 == 2'b10) & (in_1 == 2'b11) & (in_2 == 2'b11) ? 2'b10 :
(in_0 == 2'b10) & (in_1 == 2'b10) & (in_2 == 2'b11) ? 2'b10 :
(in_0 == 2'b01) & (in_1 == 2'b01) & (in_2 == 2'b10) ? 2'b10 :
(in_0 == 2'b01) & (in_1 == 2'b11) & (in_2 == 2'b10) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b10) & (in_2 == 2'b10) ? 2'b01 :
(in_0 == 2'b11) & (in_1 == 2'b01) & (in_2 == 2'b10) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b11) & (in_2 == 2'b10) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b10) & (in_2 == 2'b10) ? 2'b01 :
(in_0 == 2'b10) & (in_1 == 2'b01) & (in_2 == 2'b10) ? 2'b01 :
(in_0 == 2'b10) & (in_1 == 2'b11) & (in_2 == 2'b10) ? 2'b01 :
(in_0 == 2'b10) & (in_1 == 2'b10) & (in_2 == 2'b10) ? 2'b01 :
2'b00;
endmodule

module f_5_bet (
     input wire[1:0] in_0,
     output wire[1:0] out_0
     );

     assign out_0 = 
(in_0 == 2'b01) ? 2'b10 :
(in_0 == 2'b11) ? 2'b11 :
(in_0 == 2'b10) ? 2'b01 :
2'b00;
endmodule

module f_7AR_bet (
     input wire[1:0] in_0,
     input wire[1:0] in_1,
     output wire[1:0] out_0
     );

     assign out_0 = 
(in_0 == 2'b01) & (in_1 == 2'b01) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b11) ? 2'b11 :
(in_0 == 2'b01) & (in_1 == 2'b10) ? 2'b10 :
(in_0 == 2'b11) & (in_1 == 2'b01) ? 2'b11 :
(in_0 == 2'b11) & (in_1 == 2'b11) ? 2'b01 :
(in_0 == 2'b11) & (in_1 == 2'b10) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b01) ? 2'b11 :
(in_0 == 2'b10) & (in_1 == 2'b11) ? 2'b10 :
(in_0 == 2'b10) & (in_1 == 2'b10) ? 2'b01 :
2'b00;
endmodule


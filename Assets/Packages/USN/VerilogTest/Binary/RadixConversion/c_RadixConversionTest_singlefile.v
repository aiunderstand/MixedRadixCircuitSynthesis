module c_RadixConversionTest (
     input [2:0] io_in,
     output [5:0] io_out
);

wire bnet_0 = io_in[0];
wire [1:0] tnet_1 = io_in[2:1];

wire [1:0] tnet_2;
wire [1:0] tnet_3 = tnet_2;
wire bnet_4;
wire bnet_5 = bnet_4;

assign io_out[0] = tnet_2[1];
assign io_out[2:1] = tnet_3;
assign io_out[4:3] = {bnet_4,!bnet_4};
assign io_out[5] = bnet_5;

f_5_bet LogicGate_0 (
.in_0({bnet_0,!bnet_0}),
.out_0(tnet_2)
);

f_2 LogicGate_1 (
.in_0(tnet_1[1]),
.out_0(bnet_4)
);

endmodule

module f_2 (
     input wire in_0,
     output wire out_0
     );

     assign out_0 = 
(in_0 == 0)
;
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
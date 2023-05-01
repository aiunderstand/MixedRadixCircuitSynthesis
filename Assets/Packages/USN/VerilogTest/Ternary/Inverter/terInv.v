`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps

module c_terInv (
     input [2:0] io_in,
     output [2:0] io_out
);

wire [1:0] tnet_0 = io_in[1:0];
wire bnet_1 = io_in[2];

wire [1:0] tnet_2;
wire bnet_3;

assign io_out[1:0] = tnet_2;
assign io_out[2] = bnet_3;

f_5_bet LogicGate_0 (
.in_0(tnet_0),
.out_0(tnet_2)
);

f_2 LogicGate_1 (
.in_0(bnet_1),
.out_0(bnet_3)
);

endmodule

module f_2 (
     input wire in_0,
     output wire out_0
     );

     assign out_0 = (in_0 == 0);
endmodule

module f_5_bet (
     input wire[1:0] in_0,
     output wire[1:0] out_0
     );

     assign out_0 = (in_0 == 2'b01) ? 2'b10 :(in_0 == 2'b11) ? 2'b11 :(in_0 == 2'b10) ? 2'b01 :2'b00;
endmodule
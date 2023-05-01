`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps

module c_binInv (
     input [1:0] io_in,
     output [1:0] io_out
);

wire bnet_0 = io_in[0];
wire bnet_1 = io_in[1];

wire bnet_2;

assign io_out[1] = bnet_1;
assign io_out[0] = bnet_2;

f_2 LogicGate_0 (
.in_0(bnet_0),
.out_0(bnet_2)
);

endmodule

module f_2 (
     input wire in_0,
     output wire out_0
     );

     assign out_0 = (in_0 == 0);
endmodule

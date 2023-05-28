`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps

module c_NORLATCH (
     input [1:0] io_in,
     output [1:0] io_out
);

wire bnet_0 = io_in[1]; //Reset
wire bnet_1 = io_in[0]; //Set

wire bnet_2;
wire bnet_3;
wire bnet_4 = bnet_2;
wire bnet_5 = bnet_3;

assign io_out[1] = bnet_4; //DataOut
assign io_out[0] = bnet_5; //DataOut

f_22Z LogicGate_0 (
.portB(bnet_0),
.portA(bnet_3),
.out(bnet_2)
);

f_22Z LogicGate_1 (
.portA(bnet_1),
.portB(bnet_2),
.out(bnet_3)
);

endmodule

module f_22Z (
     input wire portB,
     input wire portA,
     output wire out
     );

     assign out = 
    (portB == 0 & portA == 0) |
    (portB == 1 & portA == 0) |
    (portB == 0 & portA == 1);
endmodule


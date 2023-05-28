`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps

module c_DNORLATCHv2 (
     input [1:0] io_in,
     output [1:0] io_out
);

wire bnet_0 = io_in[1]; //data
wire bnet_1 = io_in[0]; //clock

wire bnet_2;
wire bnet_3;
wire bnet_4;
wire bnet_5;

assign io_out[1] = bnet_4; //Q
assign io_out[0] = bnet_5; //notQ

c_Dlatchcontrol SavedGate_0 (
.io_in({bnet_0,bnet_1}),
.io_out({bnet_2,bnet_3})
);

c_NORLATCH SavedGate_1 (
.io_in({bnet_2,bnet_3}),
.io_out({bnet_4,bnet_5})
);

endmodule

module c_Dlatchcontrol (
     input [1:0] io_in,
     output [1:0] io_out
);

wire bnet_0 = io_in[1]; //Data
wire bnet_1 = bnet_0;
wire bnet_2 = io_in[0]; //Clock
wire bnet_3 = bnet_2;

wire bnet_4;
wire bnet_5;
wire bnet_6;

assign io_out[1] = bnet_5; //Reset
assign io_out[0] = bnet_6; //Set

f_22Z LogicGate_0 (
.portB(bnet_0),
.portA(bnet_2),
.out(bnet_5)
);

f_22Z LogicGate_1 (
.portB(bnet_3),
.portA(bnet_4),
.out(bnet_6)
);

f_2 LogicGate_2 (
.portA(bnet_1),
.out(bnet_4)
);

endmodule

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

assign io_out[1] = bnet_4; //Q
assign io_out[0] = bnet_5; //notQ

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

module f_2 (
     input wire portA,
     output wire out
     );

     assign out = 
    (portA == 0);
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
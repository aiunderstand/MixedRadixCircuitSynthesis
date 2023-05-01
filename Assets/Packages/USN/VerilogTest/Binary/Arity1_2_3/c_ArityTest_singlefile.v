module c_Arity123TestBinary (
     input [2:0] io_in,
     output [2:0] io_out
);

wire bnet_0 = io_in[0];
wire bnet_1 = io_in[1];
wire bnet_2 = bnet_1;
wire bnet_3 = io_in[2];
wire bnet_4 = bnet_3;
wire bnet_5 = bnet_3;

wire bnet_6;
wire bnet_7;
wire bnet_8;

assign io_out[0] = bnet_6;
assign io_out[1] = bnet_7;
assign io_out[2] = bnet_8;

f_00Z000000 LogicGate_0 (
.in_2(bnet_0),
.in_1(bnet_2),
.in_0(bnet_5),
.out_0(bnet_6)
);

f_00K LogicGate_1 (
.in_1(bnet_1),
.in_0(bnet_4),
.out_0(bnet_7)
);

f_2 LogicGate_2 (
.in_0(bnet_3),
.out_0(bnet_8)
);

endmodule

module f_00K (
     input wire in_0,
     input wire in_1,
     output wire out_0
     );

     assign out_0 = 
(in_0 == 0 & in_1 == 1)
;
endmodule

module f_00Z000000 (
     input wire in_0,
     input wire in_1,
     input wire in_2,
     output wire out_0
     );

     assign out_0 = 
(in_0 == 0 & in_1 == 0 & in_2 == 1)
 | 
(in_0 == 0 & in_1 == 1 & in_2 == 1)
;
endmodule

module f_2 (
     input wire in_0,
     output wire out_0
     );

     assign out_0 = 
(in_0 == 0)
;
endmodule


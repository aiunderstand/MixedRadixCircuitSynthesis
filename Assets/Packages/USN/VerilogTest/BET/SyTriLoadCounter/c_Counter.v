`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps
module c_CounterTest2 (
     input [7:0] io_in,
     output [3:0] io_out
);

wire bnet_0 = io_in[7]; //Clock
wire bnet_1 = bnet_0;
wire bnet_2 = io_in[6]; //Load
wire bnet_3 = bnet_2;
wire [1:0] tnet_4 = io_in[5:4]; //Data1
wire [1:0] tnet_5 = io_in[3:2]; //Data0
wire [1:0] tnet_6 = io_in[1:0]; //Dir
wire [1:0] tnet_7 = tnet_6;

wire [1:0] tnet_8;
wire [1:0] tnet_9;
wire [1:0] tnet_10;
wire [1:0] tnet_11 = tnet_9;

assign io_out[3:2] = tnet_10; //DataOut
assign io_out[1:0] = tnet_11; //DataOut

c_SyTriDirLoadCounter SavedGate_0 (
.io_in({bnet_1,bnet_3,tnet_4,tnet_8}),
.io_out({tnet_10})
);

c_CONS SavedGate_1 (
.io_in({tnet_9,tnet_7}),
.io_out({tnet_8})
);

c_SyTriDirLoadCounter SavedGate_2 (
.io_in({bnet_0,bnet_2,tnet_5,tnet_6}),
.io_out({tnet_9})
);

endmodule

module c_BTLatch (
     input [2:0] io_in,
     output [1:0] io_out
);

wire bnet_0 = io_in[2]; //DataIn
wire [1:0] tnet_1 = io_in[1:0]; //DataIn

wire [1:0] tnet_2;
wire [1:0] tnet_3 = tnet_2;

assign io_out[1:0] = tnet_3; //DataOut

f_ZD0PPPPPP_bet LogicGate_0 (
.portC({bnet_0,!bnet_0}),
.portA(tnet_1),
.portB(tnet_2),
.out(tnet_2)
);

endmodule

module c_CONS (
     input [3:0] io_in,
     output [1:0] io_out
);

wire [1:0] tnet_0 = io_in[3:2]; //DataIn
wire [1:0] tnet_1 = io_in[1:0]; //DataIn

wire [1:0] tnet_2;

assign io_out[1:0] = tnet_2; //DataOut

f_RDC_bet LogicGate_0 (
.portB(tnet_0),
.portA(tnet_1),
.out(tnet_2)
);

endmodule

module c_SyTriDirLoadCounter (
     input [5:0] io_in,
     output [1:0] io_out
);

wire bnet_0 = io_in[5]; //Clock
wire bnet_1 = io_in[4]; //LoadEn
wire [1:0] tnet_2 = io_in[3:2]; //Data
wire [1:0] tnet_3 = io_in[1:0]; //Dir

wire [1:0] tnet_4;
wire [1:0] tnet_5;
wire [1:0] tnet_6;
wire [1:0] tnet_7 = tnet_6;

assign io_out[1:0] = tnet_7; //DataOut

f_PPPPPPZD0_bet LogicGate_0 (
.portC({bnet_1,!bnet_1}),
.portB(tnet_2),
.portA(tnet_5),
.out(tnet_4)
);

f_7PB_bet LogicGate_1 (
.portB(tnet_3),
.portA(tnet_6),
.out(tnet_5)
);

c_TFF SavedGate_0 (
.io_in({bnet_0,tnet_4}),
.io_out({tnet_6})
);

endmodule

module c_TFF (
     input [2:0] io_in,
     output [1:0] io_out
);

wire bnet_0 = io_in[2]; //DataIn
wire bnet_1 = bnet_0;
wire [1:0] tnet_2 = io_in[1:0]; //DataIn

wire bnet_3;
wire [1:0] tnet_4;
wire [1:0] tnet_5;

assign io_out[1:0] = tnet_5; //DataOut

f_2 LogicGate_0 (
.portA(bnet_0),
.out(bnet_3)
);

c_BTLatch SavedGate_0 (
.io_in({bnet_1,tnet_4}),
.io_out({tnet_5})
);

c_BTLatch SavedGate_1 (
.io_in({bnet_3,tnet_2}),
.io_out({tnet_4})
);

endmodule

module f_2 (
     input wire portA,
     output wire out
     );

     assign out = 
    (portA == 0);
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

module f_PPPPPPZD0_bet (
     input wire[1:0] portC,
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portC == 2'b01) & (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b01) & (portB == 2'b11) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b01) & (portB == 2'b10) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b01) & (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b01) & (portB == 2'b11) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b01) & (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b11) & (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b11) & (portB == 2'b10) & (portA == 2'b01) ? 2'b10 :
    (portC == 2'b11) & (portB == 2'b01) & (portA == 2'b11) ? 2'b01 :
    (portC == 2'b11) & (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portC == 2'b11) & (portB == 2'b01) & (portA == 2'b10) ? 2'b01 :
    (portC == 2'b11) & (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b10) & (portB == 2'b10) & (portA == 2'b01) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b11) ? 2'b01 :
    (portC == 2'b10) & (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b10) ? 2'b01 :
    (portC == 2'b10) & (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
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

module f_ZD0PPPPPP_bet (
     input wire[1:0] portC,
     input wire[1:0] portB,
     input wire[1:0] portA,
     output reg[1:0] out
     );

     always @(posedge portC[1])
     out <= 
     (portA == 2'b01) ? 2'b01 :
     (portA == 2'b10) ? 2'b10 :
      2'b11 ;
endmodule
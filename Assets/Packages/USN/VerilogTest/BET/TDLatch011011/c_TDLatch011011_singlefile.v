`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps
module c_TDLatch011011 (
     input [2:0] io_in,
     output [1:0] io_out
);

wire bnet_0 = io_in[2]; //Enable
wire [1:0] tnet_1 = io_in[1:0]; //Data

wire [1:0] tnet_2;
wire [1:0] tnet_3 = tnet_2;

assign io_out[1:0] = tnet_3; //Out

f_ZD0PPPPPP_bet LogicGate_0 (
.portC({bnet_0,!bnet_0}),
.portA(tnet_1),
.portB(tnet_2),
.out(tnet_2)
);

endmodule

module f_ZD0PPPPPP_bet (
     input wire[1:0] portC,
     input wire[1:0] portB,
     input wire[1:0] portA,
     output wire[1:0] out
     );

     assign out = 
    (portC == 2'b01) & (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b01) & (portB == 2'b11) & (portA == 2'b01) ? 2'b11 :
    (portC == 2'b01) & (portB == 2'b10) & (portA == 2'b01) ? 2'b10 :
    (portC == 2'b01) & (portB == 2'b00) & (portA == 2'b01) ? 2'b11 : //
  
    (portC == 2'b01) & (portB == 2'b01) & (portA == 2'b11) ? 2'b01 :
    (portC == 2'b01) & (portB == 2'b11) & (portA == 2'b11) ? 2'b11 :
    (portC == 2'b01) & (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portC == 2'b01) & (portB == 2'b00) & (portA == 2'b11) ? 2'b11 : //
  
    (portC == 2'b01) & (portB == 2'b01) & (portA == 2'b10) ? 2'b01 :
    (portC == 2'b01) & (portB == 2'b11) & (portA == 2'b10) ? 2'b11 :
    (portC == 2'b01) & (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b01) & (portB == 2'b00) & (portA == 2'b10) ? 2'b11 : //
    
    (portC == 2'b01) & (portB == 2'b01) & (portA == 2'b00) ? 2'b01 : //
    (portC == 2'b01) & (portB == 2'b11) & (portA == 2'b00) ? 2'b11 : //
    (portC == 2'b01) & (portB == 2'b10) & (portA == 2'b00) ? 2'b10 : //
    (portC == 2'b01) & (portB == 2'b00) & (portA == 2'b00) ? 2'b11 : //
    
    
    (portC == 2'b11) & (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b11) & (portB == 2'b11) & (portA == 2'b01) ? 2'b11 :
    (portC == 2'b11) & (portB == 2'b10) & (portA == 2'b01) ? 2'b10 :
    (portC == 2'b11) & (portB == 2'b00) & (portA == 2'b01) ? 2'b11 : //
    
    (portC == 2'b11) & (portB == 2'b01) & (portA == 2'b11) ? 2'b01 :
    (portC == 2'b11) & (portB == 2'b11) & (portA == 2'b11) ? 2'b11 :
    (portC == 2'b11) & (portB == 2'b10) & (portA == 2'b11) ? 2'b10 :
    (portC == 2'b11) & (portB == 2'b00) & (portA == 2'b11) ? 2'b11 : //
    
    (portC == 2'b11) & (portB == 2'b01) & (portA == 2'b10) ? 2'b01 :
    (portC == 2'b11) & (portB == 2'b11) & (portA == 2'b10) ? 2'b11 :
    (portC == 2'b11) & (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b11) & (portB == 2'b00) & (portA == 2'b10) ? 2'b11 : //
  
    (portC == 2'b11) & (portB == 2'b01) & (portA == 2'b00) ? 2'b01 : //
    (portC == 2'b11) & (portB == 2'b11) & (portA == 2'b00) ? 2'b11 : //
    (portC == 2'b11) & (portB == 2'b10) & (portA == 2'b00) ? 2'b10 : //
    (portC == 2'b11) & (portB == 2'b00) & (portA == 2'b00) ? 2'b11 : //
  
  
    (portC == 2'b00) & (portB == 2'b01) & (portA == 2'b01) ? 2'b01 : //
    (portC == 2'b00) & (portB == 2'b11) & (portA == 2'b01) ? 2'b11 : //
    (portC == 2'b00) & (portB == 2'b10) & (portA == 2'b01) ? 2'b10 : //
    (portC == 2'b00) & (portB == 2'b00) & (portA == 2'b01) ? 2'b11 : //
    
    (portC == 2'b00) & (portB == 2'b01) & (portA == 2'b11) ? 2'b01 : //
    (portC == 2'b00) & (portB == 2'b11) & (portA == 2'b11) ? 2'b11 : //
    (portC == 2'b00) & (portB == 2'b10) & (portA == 2'b11) ? 2'b10 : //
    (portC == 2'b00) & (portB == 2'b00) & (portA == 2'b11) ? 2'b11 : //
    
    (portC == 2'b00) & (portB == 2'b01) & (portA == 2'b10) ? 2'b01 : //
    (portC == 2'b00) & (portB == 2'b11) & (portA == 2'b10) ? 2'b11 : //
    (portC == 2'b00) & (portB == 2'b10) & (portA == 2'b10) ? 2'b10 : //
    (portC == 2'b00) & (portB == 2'b00) & (portA == 2'b10) ? 2'b11 : //
  
    (portC == 2'b00) & (portB == 2'b01) & (portA == 2'b00) ? 2'b01 : //
    (portC == 2'b00) & (portB == 2'b11) & (portA == 2'b00) ? 2'b11 : //
    (portC == 2'b00) & (portB == 2'b10) & (portA == 2'b00) ? 2'b10 : //
    (portC == 2'b00) & (portB == 2'b00) & (portA == 2'b00) ? 2'b11 : //
    
    
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b10) & (portB == 2'b11) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b10) & (portB == 2'b10) & (portA == 2'b01) ? 2'b01 :
    (portC == 2'b10) & (portB == 2'b00) & (portA == 2'b01) ? 2'b01 : //
    
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b11) ? 2'b11 :
    (portC == 2'b10) & (portB == 2'b11) & (portA == 2'b11) ? 2'b11 :
    (portC == 2'b10) & (portB == 2'b10) & (portA == 2'b11) ? 2'b11 :
    (portC == 2'b10) & (portB == 2'b00) & (portA == 2'b11) ? 2'b11 : //
    
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b11) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b10) & (portA == 2'b10) ? 2'b10 :
    (portC == 2'b10) & (portB == 2'b00) & (portA == 2'b10) ? 2'b10 : //
    
    (portC == 2'b10) & (portB == 2'b01) & (portA == 2'b00) ? 2'b11 : //
    (portC == 2'b10) & (portB == 2'b11) & (portA == 2'b00) ? 2'b11 : //
    (portC == 2'b10) & (portB == 2'b10) & (portA == 2'b00) ? 2'b11 : //
    (portC == 2'b10) & (portB == 2'b00) & (portA == 2'b00) ? 2'b11 : //
    2'b11;
endmodule


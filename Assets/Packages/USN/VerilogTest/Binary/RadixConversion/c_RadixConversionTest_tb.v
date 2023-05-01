`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps

module test;

  /* Make a reset that pulses once. */
    reg[2:0] io_in;
    wire[5:0] io_out;
  
  c_RadixConversionTest c1 (.io_in(io_in), .io_out(io_out));
  
  // duration for each bit = 20 * timescale = 20 * 1 ns  = 20ns
  localparam delayNs = 20;  
    
  initial begin
  
      //00 is illegal for ternary, though output is same as ternary 01, so all output is high
      io_in[0] = 0; //Binary Input 
     
      io_in[2] = 0; //Ternary Input
      io_in[1] = 0;  
      #delayNs;
  
      //all input is low, all output is high
      io_in[0] = 0; //Binary Input 
      
      io_in[2] = 0; //Ternary Input
      io_in[1] = 1; 
      #delayNs;
  
      //11 is the same as ternary 10, so all output is low  
      io_in[0] = 1; //Binary Input 
      
      io_in[2] = 1; //Ternary Input
      io_in[1] = 1; 
      #delayNs;
      
      //all input is high, all output is low
      io_in[0] = 1; //Binary Input 
      
      io_in[2] = 1; //Ternary Input
      io_in[1] = 0; 
      #delayNs;
      
  end

  /* Make a regular pulsing clock. */
  //reg clk = 0;
  //always #5 clk = !clk;
  
  initial
     $monitor("At time %t, value = %h (%0d)",
              $time, io_out, io_out);
endmodule // test
`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps

module test;

  /* Make a reset that pulses once. */
    reg[7:0] io_in;
    wire[7:0] io_out;
  
  c_BTA4 c1 (.io_in(io_in), .io_out(io_out));
  
  // duration for each bit = 20 * timescale = 20 * 1 ns  = 20ns
  localparam delayNs = 20;  
     //MSB LSB
    //01 = -1
    //11 = 0
    //10 = +1
    //00 illegal / undefined
  initial begin
  //sum: -4 + -4 = -8
  //outcome: input (55) -> outcome (b7)
      io_in[7] = 0; //01: y0 = -1      
      io_in[6] = 1; 
      io_in[5] = 0; //01 y1 = -1
      io_in[4] = 1;
      io_in[3] = 0; //01 x0 = -1
      io_in[2] = 1;
      io_in[1] = 0; //01 x1 = -1     
      io_in[0] = 1;
     
      #delayNs;
  
    
  end

  /* Make a regular pulsing clock. */
  //reg clk = 0;
  //always #5 clk = !clk;
  
  initial
     $monitor("At time %t, value = %h (%0d)",
              $time, io_out, io_out);
endmodule // test
`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps

module test;

  /* Make a reset that pulses once. */
    reg[2:0] io_in;
    wire[2:0] io_out;
  
  c_terInv c1 (.io_in(io_in), .io_out(io_out));
  
  // duration for each bit = 20 * timescale = 20 * 1 ns  = 20ns
  localparam delayNs = 20;  
    
  initial begin
      io_in[0] = 0;
      io_in[1] = 1;      
      io_in[2] = 0;
    
      #delayNs;
      io_in[0] = 1;
      io_in[1] = 1;      
      io_in[2] = 0;
      
      #delayNs;
      io_in[0] = 1;
      io_in[1] = 0;      
      io_in[2] = 0;
      
      #delayNs;
      io_in[0] = 0;
      io_in[1] = 0;      
      io_in[2] = 1;
      
      #delayNs;
  end

  /* Make a regular pulsing clock. */
  //reg clk = 0;
  //always #5 clk = !clk;
  
  initial
     $monitor("At time %t, value = %h (%0d)",
              $time, io_out, io_out);
endmodule // test
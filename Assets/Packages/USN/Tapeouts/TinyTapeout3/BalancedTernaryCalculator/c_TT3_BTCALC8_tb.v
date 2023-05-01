`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps

module test;

  /* Make a reset that pulses once. */
    reg[7:0] io_in;
    wire[7:0] io_out;
  
  c_TT3_BTCalc8 c1 (.io_in(io_in), .io_out(io_out));
  
  // duration for each bit = 20 * timescale = 20 * 1 ns  = 20ns
  localparam delayNs = 20;  
    
  //balanced ternary encoding notation. 
  //01 is logical -1
  //11 is logical 0 
  //10 is logical 1
  //00 is illegal / undefined 
  
  initial begin
  //FUNCTION ADDITION/SUBTRACTION
  //balanced ternary positional numbering system notation: 
  //-1x 3^1 + -1x 3^0  plus -1x 3^1 + -1x 3^0 = -4 + -4 = -8  
  //in short: -- plus -- = -4 + -4 = -8
  //note the confusing order! eg. port 7 is y0 High, port 6 is y0 port Low. 
  //The interface will change in a future version.
   
      //input (55) -> output (b7) = -4 -4 = -8
      io_in[7] = 0; //01: y0 = -1   [-1 is addition/subtraction, 0 and 1 is multiplication]     
      io_in[6] = 1; 
      io_in[5] = 0; //01 y1 = -1
      io_in[4] = 1;
      
      io_in[3] = 0; //01 x0 = -1
      io_in[2] = 1;
      io_in[1] = 0; //01 x1 = -1     
      io_in[0] = 1;
      
      #delayNs;
      
  //FUNCTION MULTIPLICATION
  //balanced ternary sum of -0 plus -- = -3 * -4 = 12 (note double negation)
  //input (d5) -> output (eb) = -3 * -4 = 12
      io_in[7] = 1; //00: y0 = 0   [-1 is addition/subtraction, 0 and 1 is multiplication]     
      io_in[6] = 1; 
      io_in[5] = 0; //01 y1 = -1
      io_in[4] = 1;
      
      io_in[3] = 0; //01 x0 = -1
      io_in[2] = 1;
      io_in[1] = 0; //01 x1 = -1     
      io_in[0] = 1;
      
      #delayNs;
      //input (d5) -> output (eb) = -3 * -2 = 6
      io_in[7] = 1; //11: y0 = 0   [-1 is addition/subtraction, 0 and 1 is multiplication]     
      io_in[6] = 1; 
      io_in[5] = 0; //01 y1 = -1
      io_in[4] = 1;
      io_in[3] = 1; //01 x0 = 1
      io_in[2] = 0;
      io_in[1] = 0; //01 x1 = -1
      io_in[0] = 1;
      
      #delayNs;
      
      //input (95) -> output (7b) = -2 * -4 = 8 (note double negation)
      //7b is the reverse of b7 found in the first test
      //in short: -+ plus -- = -2 * -4 = 8
      io_in[7] = 1; //10: y0 = -1   [-1 is addition/subtraction, 0 and 1 is multiplication]     
      io_in[6] = 0; 
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
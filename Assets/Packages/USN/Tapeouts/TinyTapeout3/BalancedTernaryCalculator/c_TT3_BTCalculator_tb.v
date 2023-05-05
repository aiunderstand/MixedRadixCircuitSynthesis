`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps

module test;

  /* Make a reset that pulses once. */
    reg[7:0] io_in;
    wire[7:0] io_out;
  
  c_TT3_BTCalculator c1 (.io_in(io_in), .io_out(io_out));
  
  // duration for each bit = 20 * timescale = 20 * 1 ns  = 20ns
  localparam delayNs = 20;  
    
  //balanced ternary encoding notation. 
  //01 is logical -1
  //11 is logical 0 
  //10 is logical 1
  //00 is illegal / undefined 
  
  initial begin
      
  
  //FUNCTION MULTIPLICATION
  //balanced ternary sum of -0 plus -- = -3 * -4 = 12 (note double negation)
  //input (d5) -> output (eb) = -3 * -4 = 12
  
  //balanced ternary positional numbering system notation: 
  //-1x 3^1 + -1x 3^0  multiply -1x 3^1 + -1x 3^0 = -4 * -4 = 16  
  //in short: -- multiply -- = -4 * -4 = 16
   
      //input (55) -> output (96) = -4 -4 = 16 (note double negation)
      io_in[7:6] = 2'b01; //x1 = -1  [-1 is multiplication, 0 and 1 is addition/subtraction]     
      io_in[5:4] = 2'b01; //x0 = -1      
      
      io_in[3:2] = 2'b01; //y1 = -1      
      io_in[1:0] = 2'b01; //y0 = -1      
     
      #delayNs;

      
      //input (79) -> output (db) = -3 * 2 = -6
      io_in[7:6] = 2'b01; //x1 = -1  [-1 is multiplication, 0 and 1 is addition/subtraction]     
      io_in[5:4] = 2'b11; //x0 =  0      
      
      io_in[3:2] = 2'b10; //y1 = 1      
      io_in[1:0] = 2'b01; //y0 = -1      
      
      #delayNs;
      
      //FUNCTION ADDITION/SUBTRACTION
      //balanced ternary sum of 0- plus -- = -1 + -4 = -5 
      //input (d5) -> output (da) = -1 + -4 = -5
      io_in[7:6] = 2'b11; //x1 = 0  [-1 is multiplication, 0 and 1 is addition/subtraction]     
      io_in[5:4] = 2'b01; //x0 = -1      
      
      io_in[3:2] = 2'b01; //y1 = -1      
      io_in[1:0] = 2'b01; //y0 = -1    
            
      #delayNs;
      
      //input (aa) -> output (ed) = 4 + 4 = 8
      io_in[7:6] = 2'b10; //x1 = 1  [-1 is multiplication, 0 and 1 is addition/subtraction]     
      io_in[5:4] = 2'b10; //x0 = 1      
      
      io_in[3:2] = 2'b10; //y1 = 1      
      io_in[1:0] = 2'b10; //y0 = 1    
            
      #delayNs;
  end

  /* Make a regular pulsing clock. */
  //reg clk = 0;
  //always #5 clk = !clk;
  
  initial
     $monitor("At time %t, value = %h (%0d)",
              $time, io_out, io_out);
endmodule // test
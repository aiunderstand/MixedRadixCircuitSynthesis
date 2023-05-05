`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps

module test;

  reg[7:0] io_in;
  wire[7:0] io_out;
  
  c_BTM4 c1 (.io_in(io_in), .io_out(io_out));
  
  //duration for each bit = 20 * timescale = 20 * 1 ns  = 20ns
  localparam delayNs = 20;  
  
  //MSB LSB
  //01 = -1
  //11 = 0
  //10 = +1
  //00 illegal / undefined
  initial begin

  //FUNCTION MULTIPLICATION
  //input (55) -> output (96) = -4 * -4 = 16
  io_in[7] = 0; //x1     
  io_in[6] = 1; 
  io_in[5] = 0; //x0
  io_in[4] = 1;
      
  io_in[3] = 0; //y1
  io_in[2] = 1;
  io_in[1] = 0; //y0   
  io_in[0] = 1;
      
  #delayNs;
  
  //input (57) -> output (eb) = -4 * -3 = 12
  io_in[7] = 0; //x1     
  io_in[6] = 1; 
  io_in[5] = 0; //x0
  io_in[4] = 1;
      
  io_in[3] = 0; //y1
  io_in[2] = 1;
  io_in[1] = 1; //y0   
  io_in[0] = 1;
      
  #delayNs;
  
  //input (56) -> output (ed) = -4 * -2 = 8
  io_in[7] = 0; //x1     
  io_in[6] = 1; 
  io_in[5] = 0; //x0
  io_in[4] = 1;
      
  io_in[3] = 0; //y1
  io_in[2] = 1;
  io_in[1] = 1; //y0   
  io_in[0] = 0;
      
  #delayNs;

  //input (76) -> output (e7) = -3 * -2 = 6
  io_in[7] = 0; //x1     
  io_in[6] = 1; 
  io_in[5] = 1; //x0
  io_in[4] = 1;
      
  io_in[3] = 0; //y1
  io_in[2] = 1;
  io_in[1] = 1; //y0   
  io_in[0] = 0;
      
  #delayNs;
 
  //input (7f) -> output (ff) = -3 * 0 = 0
  io_in[7] = 0; //x1     
  io_in[6] = 1; 
  io_in[5] = 1; //x0
  io_in[4] = 1;
      
  io_in[3] = 1; //y1
  io_in[2] = 1;
  io_in[1] = 1; //y0   
  io_in[0] = 1;
      
  #delayNs;
    
  //input (79) -> output (db) = -3 * 2 = -6
  io_in[7] = 0; //x1     
  io_in[6] = 1; 
  io_in[5] = 1; //x0
  io_in[4] = 1;
      
  io_in[3] = 1; //y1
  io_in[2] = 0;
  io_in[1] = 0; //y0   
  io_in[0] = 1;
      
  #delayNs;
  
  //input (97) -> output (db) = 2 * -3 = -6
  io_in[7] = 1; //x1     
  io_in[6] = 0; 
  io_in[5] = 0; //x0
  io_in[4] = 1;
      
  io_in[3] = 0; //y1
  io_in[2] = 1;
  io_in[1] = 1; //y0   
  io_in[0] = 1;
      
  #delayNs;
    
  //input (6b) -> output (db) = -2 * 3 = -6
  io_in[7] = 0; //x1     
  io_in[6] = 1; 
  io_in[5] = 1; //x0
  io_in[4] = 0;
      
  io_in[3] = 1; //y1
  io_in[2] = 0;
  io_in[1] = 1; //y0   
  io_in[0] = 1;
      
  #delayNs;
  
//input (b6) -> output (db) = 3 * -2 = -6
  io_in[7] = 1; //x1     
  io_in[6] = 0; 
  io_in[5] = 1; //x0
  io_in[4] = 1;
      
  io_in[3] = 0; //y1
  io_in[2] = 1;
  io_in[1] = 1; //y0   
  io_in[0] = 0;
      
  #delayNs;
  
  //input (a5) -> output (69) = 4 * -4 = -16
  io_in[7] = 1; //x1     
  io_in[6] = 0; 
  io_in[5] = 1; //x0
  io_in[4] = 0;
      
  io_in[3] = 0; //y1
  io_in[2] = 1;
  io_in[1] = 0; //y0   
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

      
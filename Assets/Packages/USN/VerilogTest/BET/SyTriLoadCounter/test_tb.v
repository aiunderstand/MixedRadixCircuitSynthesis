`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps

module test;

    reg[7:0] io_in;
    wire[3:0] io_out;
  
  c_CounterTest2 c1 (.io_in(io_in), .io_out(io_out));
  
  // duration for each bit = 20 * timescale = 20 * 1 ns  = 20ns
  localparam delayNs = 20;  
    
  initial begin
      io_in = 8'b00000000; //illegal setup state
      #delayNs;
      io_in = 8'b10000000; //cycle
      #delayNs;
  
//      io_in = 8'b01111111; //load all zero 
//      #delayNs;
//      io_in = 8'b11111111; //cycle 
//      #delayNs;
      
      io_in = 8'b00111110; //set counter to count up 
      #delayNs;
      io_in = 8'b10111110; //cycle
      #delayNs;
      
      io_in = 8'b00111110; //set counter to count up 
      #delayNs;
      io_in = 8'b10111110; //cycle
      #delayNs;
      
      io_in = 8'b00111110; //set counter to count up 
      #delayNs;
      io_in = 8'b10111110; //cycle
      #delayNs;
      
      io_in = 8'b00111110; //set counter to count up 
      #delayNs;
      io_in = 8'b10111110; //cycle
      #delayNs;
      
      io_in = 8'b00111110; //set counter to count up 
      #delayNs;
      io_in = 8'b10111110; //cycle
      #delayNs;
    
  end

  /* Make a regular pulsing clock. */
  //reg clk = 0;
  //always #5 clk = !clk;
  
  initial
     $monitor("At time %t, value = %h (%0d)",
              $time, io_out, io_out);
endmodule // test
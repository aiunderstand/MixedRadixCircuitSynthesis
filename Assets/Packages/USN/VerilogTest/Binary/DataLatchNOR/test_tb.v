`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps

module testdlatch;

  /* Make a reset that pulses once. */
    reg[1:0] io_in;
    wire[1:0] io_out;
  
  c_DNORLATCHv2 c1 (.io_in(io_in), .io_out(io_out));
  
  // duration for each bit = 20 * timescale = 20 * 1 ns  = 20ns
  localparam delayNs = 20;  
    
  initial begin
      io_in[0] = 0; //Clock V16 (u19 Q, v19 notQ)
      io_in[1] = 0; //Data V17
    
      #delayNs;
         
      io_in[0] = 1;
      io_in[1] = 0;    
      
      #delayNs;
      
      io_in[0] = 0;
      io_in[1] = 0; 
     
      #delayNs;
      
      io_in[0] = 1;
      io_in[1] = 1; 
        
      #delayNs;
      
      io_in[0] = 0;
      io_in[1] = 1; 
      
      #delayNs;
      
      io_in[0] = 1;
      io_in[1] = 0; 
        
      #delayNs;
      
      
      io_in[0] = 0;
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
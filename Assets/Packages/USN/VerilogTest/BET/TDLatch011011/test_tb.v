`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps

module test;

    reg[2:0] io_in;
    wire[1:0] io_out;
  
  c_TDLatch011011 c1 (.io_in(io_in), .io_out(io_out));
  
  // duration for each bit = 20 * timescale = 20 * 1 ns  = 20ns
  localparam delayNs = 20;  
    
  initial begin
  //note this is an illegal state! inputs 0 and 1 can never be both be zero 
  //simulation seems to work if given long setup time, althoug at the last transistion something is wrong but not on the fpga. 
  //that transistion must have gone through 00 enforcing the 11 state. We could prevent this by exploding our codee to explain what to do in the 00 state. 
      io_in[2] = 0; //enable  
      io_in[1] = 0; //data     
      io_in[0] = 0;
      
      #delayNs;
      
      io_in[2] = 0; //enable  
      io_in[1] = 0; //data     
      io_in[0] = 1;
      
      #delayNs;
      io_in[2] = 1; //enable  
      io_in[1] = 0; //data     
      io_in[0] = 1;
    
      #delayNs;
       
      io_in[2] = 0; //enable  
      io_in[1] = 1; //data     
      io_in[0] = 1;
    
      #delayNs;
      io_in[2] = 1; //enable  
      io_in[1] = 1; //data     
      io_in[0] = 1;
    
      #delayNs;
    
      io_in[2] = 0; //enable  
      io_in[1] = 1; //data     
      io_in[0] = 0;
    
      #delayNs;
      io_in[2] = 1; //enable  
      io_in[1] = 1; //data     
      io_in[0] = 0;
     
      #delayNs;
      io_in[2] = 0; //enable  
      io_in[1] = 1; //data     
      io_in[0] = 0;
    
      #delayNs;
     
      
  end

  /* Make a regular pulsing clock. */
  //reg clk = 0;
  //always #5 clk = !clk;
  
  initial
     $monitor("At time %t, value = %h (%0d)",
              $time, io_out, io_out);
endmodule // test
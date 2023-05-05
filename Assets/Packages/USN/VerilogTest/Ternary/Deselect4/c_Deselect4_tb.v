`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps

module test;

  reg[16:0] io_in;
  wire[7:0] io_out;
  
  c_Deselect4 c1 (.io_in(io_in), .io_out(io_out));
  
  //duration for each bit = 20 * timescale = 20 * 1 ns  = 20ns
  localparam delayNs = 20;  
  
  //MSB LSB
  //01 = -1
  //11 = 0
  //10 = +1
  //00 illegal / undefined
  initial begin

  io_in[16] = 0; //select input a
    
  io_in[15:14] = 2'b10; //a3
  io_in[13:12] = 2'b01; //a2
  io_in[11:10] = 2'b01; //a1
  io_in[9:8]   = 2'b10; //a0
   
  io_in[7:6]   = 2'b01; //a3
  io_in[5:4]   = 2'b10; //a2
  io_in[3:2]   = 2'b10; //a1
  io_in[1:0]   = 2'b01; //a0
  
  #delayNs;
  
  io_in[16] = 1; //select input b
    
  io_in[15:14] = 2'b10; //a3
  io_in[13:12] = 2'b01; //a2
  io_in[11:10] = 2'b01; //a1
  io_in[9:8]   = 2'b10; //a0
   
  io_in[7:6]   = 2'b01; //a3
  io_in[5:4]   = 2'b10; //a2
  io_in[3:2]   = 2'b10; //a1
  io_in[1:0]   = 2'b01; //a0
      
  #delayNs;
 
  end


  /* Make a regular pulsing clock. */
  //reg clk = 0;
  //always #5 clk = !clk;
  
  initial
     $monitor("At time %t, value = %h (%0d)",
              $time, io_out, io_out);
endmodule // test

      
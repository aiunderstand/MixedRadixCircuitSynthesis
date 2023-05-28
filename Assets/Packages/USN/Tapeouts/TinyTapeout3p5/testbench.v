`timescale 1 ns/10 ps  // time-unit = 1 ns, precision = 10 ps

module test;

  /* Make a reset that pulses once. */
    reg[7:0] ui_in;
    reg[7:0] uio_in;
    
    wire[7:0] uo_out;
    wire [7:0] uio_out;
    reg clk;
    reg rst_n;
     
  c_tt_um_ternaryPC_radixconvert c1 (
  .ui_in(ui_in),
  .uio_in(uio_in),
  .uo_out(uo_out),
  .uio_out(uio_out),
  
  .clk(clk),
  .rst_n(rst_n));
  
  // duration for each bit = 20 * timescale = 20 * 1 ns  = 20ns
  localparam delayNs = 20;  
    
  //balanced ternary encoding notation. 
  //01 is logical -1
  //11 is logical 0 
  //10 is logical 1
  //00 is illegal / undefined 
  
  initial begin
      
      //set count registers to all logical zero (note the range is -40, 40 and it is set at 0)
      uio_in[7:6] =  2'b11;//this is the most significant trit (d3)
      ui_in [7:2] = 6'b111111; //these are in order d2, d1, d0
      
      //set count direction to count up 2'b10 is count up, 2'b01 is count down and 2'b11 is no count/hold
      ui_in [1:0] = 2'b10;
    
      //set load data flag (overwrites count)
      rst_n = 1;
      
      //set clock (writing at rising edge)
      clk =0;
      #delayNs;
     
      clk =1;
      #delayNs;
  
      //disable load flag  
      rst_n = 0;
      clk =0;
      #delayNs;
      clk =1;
      #delayNs;
    
      //add a bunch of times
      clk =0;
      #delayNs;
      clk =1;
      #delayNs;
    
      clk =0;
      #delayNs;
      clk =1;
      #delayNs;
    
      clk =0;
      #delayNs;
      clk =1;
      #delayNs;
    
      clk =0;
      #delayNs;
      clk =1;
  
      //reverse count dir
      ui_in [1:0] = 2'b01;
      #delayNs;
  
       clk =0;
       #delayNs;
       clk =1;
       #delayNs;
       
       clk =0;
       #delayNs;
       clk =1;
       #delayNs;
       
       clk =0;
       #delayNs;
       clk =1;
       #delayNs;
       
       clk =0;
       #delayNs;
       clk =1;
       #delayNs;
  end

  initial
     $monitor("At time %t, value = %h (%0d)",
              $time, uo_out, uo_out);
endmodule // test
## Voltage config
set_property CONFIG_VOLTAGE 3.3 [current_design]
set_property CFGBVS VCCO [current_design]

## Pin config
set_property IOSTANDARD LVCMOS33 [get_ports {ui_in[0]}]
set_property IOSTANDARD LVCMOS33 [get_ports {ui_in[1]}]
set_property IOSTANDARD LVCMOS33 [get_ports {ui_in[2]}]
set_property IOSTANDARD LVCMOS33 [get_ports {ui_in[3]}]
set_property IOSTANDARD LVCMOS33 [get_ports {ui_in[4]}]
set_property IOSTANDARD LVCMOS33 [get_ports {ui_in[5]}]
set_property IOSTANDARD LVCMOS33 [get_ports {ui_in[6]}]
set_property IOSTANDARD LVCMOS33 [get_ports {ui_in[7]}]

set_property IOSTANDARD LVCMOS33 [get_ports {uio_in[6]}]
set_property IOSTANDARD LVCMOS33 [get_ports {uio_in[7]}]
set_property IOSTANDARD LVCMOS33 [get_ports {rst_n}]
set_property IOSTANDARD LVCMOS33 [get_ports {clk}]

set_property IOSTANDARD LVCMOS33 [get_ports {uo_out[0]}]
set_property IOSTANDARD LVCMOS33 [get_ports {uo_out[1]}]
set_property IOSTANDARD LVCMOS33 [get_ports {uo_out[2]}]
set_property IOSTANDARD LVCMOS33 [get_ports {uo_out[3]}]
set_property IOSTANDARD LVCMOS33 [get_ports {uo_out[4]}]
set_property IOSTANDARD LVCMOS33 [get_ports {uo_out[5]}]
set_property IOSTANDARD LVCMOS33 [get_ports {uo_out[6]}]
set_property IOSTANDARD LVCMOS33 [get_ports {uo_out[7]}]

set_property IOSTANDARD LVCMOS33 [get_ports {uio_out[0]}]
set_property IOSTANDARD LVCMOS33 [get_ports {uio_out[1]}]
set_property IOSTANDARD LVCMOS33 [get_ports {uio_out[2]}]
set_property IOSTANDARD LVCMOS33 [get_ports {uio_out[3]}]
set_property IOSTANDARD LVCMOS33 [get_ports {uio_out[4]}]
set_property IOSTANDARD LVCMOS33 [get_ports {uio_out[5]}]

##added although not used
set_property IOSTANDARD LVCMOS33 [get_ports {uio_out[6]}] 
set_property IOSTANDARD LVCMOS33 [get_ports {uio_out[7]}]


## Pin assignment
set_property PACKAGE_PIN V17 [get_ports {ui_in[0]}]
set_property PACKAGE_PIN V16 [get_ports {ui_in[1]}]
set_property PACKAGE_PIN W16 [get_ports {ui_in[2]}]
set_property PACKAGE_PIN W17 [get_ports {ui_in[3]}]
set_property PACKAGE_PIN W15 [get_ports {ui_in[4]}]
set_property PACKAGE_PIN V15 [get_ports {ui_in[5]}]
set_property PACKAGE_PIN W14 [get_ports {ui_in[6]}]
set_property PACKAGE_PIN W13 [get_ports {ui_in[7]}]

set_property PACKAGE_PIN V2 [get_ports {uio_in[6]}]
set_property PACKAGE_PIN T3 [get_ports {uio_in[7]}]
set_property PACKAGE_PIN T2 [get_ports {rst_n}]
set_property PACKAGE_PIN R3 [get_ports {clk}]


set_property PACKAGE_PIN U16 [get_ports {uo_out[0]}]
set_property PACKAGE_PIN E19 [get_ports {uo_out[1]}]
set_property PACKAGE_PIN U19 [get_ports {uo_out[2]}]
set_property PACKAGE_PIN V19 [get_ports {uo_out[3]}]
set_property PACKAGE_PIN W18 [get_ports {uo_out[4]}]
set_property PACKAGE_PIN U15 [get_ports {uo_out[5]}]
set_property PACKAGE_PIN U14 [get_ports {uo_out[6]}]
set_property PACKAGE_PIN V14 [get_ports {uo_out[7]}]

##54 = RC2, 32 = RC1, 10= RC0
set_property PACKAGE_PIN V13 [get_ports {uio_out[0]}]
set_property PACKAGE_PIN V3 [get_ports {uio_out[1]}]
set_property PACKAGE_PIN W3 [get_ports {uio_out[2]}]
set_property PACKAGE_PIN U3 [get_ports {uio_out[3]}]
set_property PACKAGE_PIN P3 [get_ports {uio_out[4]}]
set_property PACKAGE_PIN N3 [get_ports {uio_out[5]}] 

#added although not used
set_property PACKAGE_PIN P1 [get_ports {uio_out[6]}]
set_property PACKAGE_PIN L1 [get_ports {uio_out[7]}]

## Combinatorial Loop config (custom latches and FFs)
##set_property ALLOW_COMBINATORIAL_LOOPS true [get_nets -segments -of_objects [get_cells LogicGate_0]] 
##set_property ALLOW_COMBINATORIAL_LOOPS true [get_nets -of_objects [get_cells SavedGate_0/SavedGate_LogicGate_0]] 
#set_property SEVERITY {Warning}  [get_drc_checks LUTLP-1] 
#set_property SEVERITY {Warning} [get_drc_checks NSTD-1]
set_property CLOCK_DEDICATED_ROUTE FALSE [get_nets clk_IBUF]
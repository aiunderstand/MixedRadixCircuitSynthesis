## Voltage config
set_property CONFIG_VOLTAGE 3.3 [current_design]
set_property CFGBVS VCCO [current_design]

## Pin config
set_property IOSTANDARD LVCMOS33 [get_ports {io_in[0]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_in[1]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_in[2]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_in[3]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_in[4]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_in[5]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_in[6]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_in[7]}]

set_property IOSTANDARD LVCMOS33 [get_ports {io_out[0]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_out[1]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_out[2]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_out[3]}]

## Pin assignment
set_property PACKAGE_PIN V17 [get_ports {io_in[0]}]
set_property PACKAGE_PIN V16 [get_ports {io_in[1]}]
set_property PACKAGE_PIN W16 [get_ports {io_in[2]}]
set_property PACKAGE_PIN W17 [get_ports {io_in[3]}]
set_property PACKAGE_PIN W15 [get_ports {io_in[4]}]
set_property PACKAGE_PIN V15 [get_ports {io_in[5]}]
set_property PACKAGE_PIN W14 [get_ports {io_in[6]}]
set_property PACKAGE_PIN W13 [get_ports {io_in[7]}]

set_property PACKAGE_PIN U16 [get_ports {io_out[0]}]
set_property PACKAGE_PIN E19 [get_ports {io_out[1]}]
set_property PACKAGE_PIN U19 [get_ports {io_out[2]}]
set_property PACKAGE_PIN V19 [get_ports {io_out[3]}]

## Combinatorial Loop config (custom latches and FFs)
##set_property ALLOW_COMBINATORIAL_LOOPS true [get_nets -segments -of_objects [get_cells LogicGate_0]] 
#set_property ALLOW_COMBINATORIAL_LOOPS true [get_nets -of_objects [get_cells SavedGate_2/SavedGate_0/SavedGate_0/LogicGate_0]] 
#set_property ALLOW_COMBINATORIAL_LOOPS true [get_nets -of_objects [get_cells SavedGate_0/SavedGate_0/SavedGate_0/LogicGate_0]] 

#set_property SEVERITY {Warning}  [get_drc_checks LUTLP-1] 
#set_property SEVERITY {Warning} [get_drc_checks NSTD-1]
set_property CLOCK_DEDICATED_ROUTE FALSE [get_nets io_in_IBUF[7]]
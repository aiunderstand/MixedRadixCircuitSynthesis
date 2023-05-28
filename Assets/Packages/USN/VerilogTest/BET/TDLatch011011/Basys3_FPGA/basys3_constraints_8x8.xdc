## Voltage config
set_property CONFIG_VOLTAGE 3.3 [current_design]
set_property CFGBVS VCCO [current_design]

## Pin config
set_property IOSTANDARD LVCMOS33 [get_ports {io_in[0]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_in[1]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_in[2]}]

set_property IOSTANDARD LVCMOS33 [get_ports {io_out[0]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_out[1]}]

## Pin assignment
set_property PACKAGE_PIN V17 [get_ports {io_in[0]}]
set_property PACKAGE_PIN V16 [get_ports {io_in[1]}]
set_property PACKAGE_PIN W16 [get_ports {io_in[2]}]

set_property PACKAGE_PIN U16 [get_ports {io_out[0]}]
set_property PACKAGE_PIN E19 [get_ports {io_out[1]}]

## Combinatorial Loop config (custom latches and FFs)
##set_property ALLOW_COMBINATORIAL_LOOPS true [get_nets -segments -of_objects [get_cells LogicGate_0]] 
set_property ALLOW_COMBINATORIAL_LOOPS true [get_nets -of_objects [get_cells LogicGate_0]] 
set_property SEVERITY {Warning}  [get_drc_checks LUTLP-1] 
set_property SEVERITY {Warning} [get_drc_checks NSTD-1]
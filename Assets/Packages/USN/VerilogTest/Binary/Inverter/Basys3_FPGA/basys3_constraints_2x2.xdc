set_property IOSTANDARD LVCMOS33 [get_ports {io_in[1]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_in[0]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_out[1]}]
set_property IOSTANDARD LVCMOS33 [get_ports {io_out[0]}]

set_property PACKAGE_PIN V17 [get_ports {io_in[1]}]
set_property PACKAGE_PIN V16 [get_ports {io_in[0]}]
set_property PACKAGE_PIN U19 [get_ports {io_out[1]}]
set_property PACKAGE_PIN V19 [get_ports {io_out[0]}]

set_property CONFIG_VOLTAGE 3.3 [current_design]
set_property CFGBVS VCCO [current_design]
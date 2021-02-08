.subckt circuit DataIn0_-4520 DataIn1_-4520 DataIn2_-4520 DataOut0_-7310 vdd
.lib 'CNFET.lib' CNFET 
.include "f_GZG.sp"
.include "f_ABC.sp"
.include "nti.sp" 
.include "pti.sp"

xnti0 DataIn2_-4520 DataIn2_-4520_n vdd nti
xnti1 PortD-42634_to_PortB21992 PortD-42634_to_PortB21992_n vdd nti

xckt0 DataIn2_-4520 DataIn2_-4520_n PortD-42634_to_PortB21992 PortD-42634_to_PortB21992_n DataOut0_-7310 vdd f_GZG

xpti2 DataIn1_-4520 DataIn1_-4520_p vdd pti
xnti3 DataIn1_-4520 DataIn1_-4520_n vdd nti
xnti4 DataIn0_-4520 DataIn0_-4520_n vdd nti

xckt1 DataIn1_-4520 DataIn1_-4520_p DataIn1_-4520_n DataIn0_-4520 DataIn0_-4520_n PortD-42634_to_PortB21992 vdd f_ABC



.ends


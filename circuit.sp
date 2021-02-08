.subckt circuit -4520DataIn:0 -4520DataIn:1 -4520DataIn:2 -7310DataOut:0 vdd
.lib 'CNFET.lib' CNFET 
.include "f_GGD.sp"
.include "f_TTZ.sp"
.include "nti.sp" 
.include "pti.sp"

xnti0 -4520DataIn:1 -4520DataIn:1_n vdd nti
xnti1 -4520DataIn:0 -4520DataIn:0_n vdd nti

xckt0 -4520DataIn:1 -4520DataIn:1_n -4520DataIn:0_n -12282PortD_to_-15862PortB vdd f_GGD

xpti2 -4520DataIn:2 -4520DataIn:2_p vdd pti

xckt1 -4520DataIn:2 -4520DataIn:2_p -12282PortD_to_-15862PortB -7310DataOut:0 vdd f_TTZ



.ends


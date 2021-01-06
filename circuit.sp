.subckt circuit -4482b2 -4482b1 -4482b0 -127414PortD -139878PortD -143204PortD vdd
.lib 'CNFET.lib' CNFET 
.include "f_ED4.sp"
.include "f_8RC.sp"
.include "f_EDDDDDDDD.sp"
.include "f_8RR.sp"
.include "f_RRRRDDRDD.sp"
.include "f_840.sp"
.include "nti.sp" 
.include "pti.sp"

xpti0 -4482b1 -4482b1_p vdd pti
xnti1 -4482b1 -4482b1_n vdd nti

xckt0 -4482b0 -4482b1_p -4482b1_n -127414PortA vdd f_ED4

xpti2 21814PortD 21814PortD_p vdd pti
xnti3 21814PortD 21814PortD_n vdd nti
xpti4 -4482b2 -4482b2_p vdd pti
xnti5 -4482b2 -4482b2_n vdd nti

xckt1 21814PortD 21814PortD_p 21814PortD_n -4482b2 -4482b2_p -4482b2_n -127414PortD vdd f_8RC


xckt2 -4482b0 -4482b1_p -139878PortC vdd f_EDDDDDDDD


xckt3 -4482b1 -4482b1_p -4482b2 -4482b2_p -143204PortA vdd f_8RR


xckt4 -4482b1_p -4482b2_p -139878PortD vdd f_RRRRDDRDD

xpti6 -130752PortD -130752PortD_p vdd pti
xnti7 -130752PortD -130752PortD_n vdd nti

xckt5 -134084PortD -130752PortD_p -130752PortD_n -143204PortD vdd f_840



.ends


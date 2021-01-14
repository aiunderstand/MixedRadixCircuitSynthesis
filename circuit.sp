.subckt circuit -4510Set(w1) -4510Reset(w2) 21982PortD -73530PortD vdd
.lib 'CNFET.lib' CNFET 
.include "f_P.sp"
.include "f_PC0.sp"
.include "f_ZRP.sp"
.include "nti.sp" 
.include "pti.sp"

xpti0 -70080PortD -70080PortD_p vdd pti
xnti1 -70080PortD -70080PortD_n vdd nti

xckt0 -70080PortD_p -70080PortD_n -66630PortA vdd f_P

xpti2 -4510Set(w1) -4510Set(w1)_p vdd pti
xnti3 -4510Set(w1) -4510Set(w1)_n vdd nti

xckt1 -4510Set(w1)_p -4510Set(w1)_n -66630PortB vdd f_P

xpti4 21982PortD 21982PortD_p vdd pti
xnti5 21982PortD 21982PortD_n vdd nti
xpti6 -63180PortD -63180PortD_p vdd pti
xnti7 -63180PortD -63180PortD_n vdd nti

xckt2 21982PortD_p 21982PortD_n -63180PortD_p -63180PortD_n -73530PortA vdd f_PC0

xpti8 -76980PortD -76980PortD_p vdd pti
xnti9 -76980PortD -76980PortD_n vdd nti
xpti10 -73530PortD -73530PortD_p vdd pti
xnti11 -73530PortD -73530PortD_n vdd nti

xckt3 -76980PortD_p -76980PortD_n -73530PortD_p -73530PortD_n 21982PortA vdd f_PC0

xpti12 -66630PortD -66630PortD_p vdd pti
xnti13 -66630PortD -66630PortD_n vdd nti

xckt4 -66630PortD_p -66630PortD_n -70080PortB vdd f_P

xpti14 -4510Reset(w2) -4510Reset(w2)_p vdd pti
xnti15 -4510Reset(w2) -4510Reset(w2)_n vdd nti

xckt5 -4510Reset(w2)_p -4510Reset(w2)_n -4510Set(w1)_p -4510Set(w1)_n -70080PortA vdd f_ZRP



.ends


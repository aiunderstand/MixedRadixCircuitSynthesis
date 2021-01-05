.subckt circuit in0 in1 in2 out0 out1 vdd
.lib 'CNFET.lib' CNFET 
.include "f_20K.sp"
.include "f_K00.sp"
.include "f_ZKK.sp"
.include "nti.sp" 
.include "pti.sp"

xpti0 -4520inputb -4520inputb_p vdd pti
xnti1 -4520inputb -4520inputb_n vdd nti
xpti2 -4520inputcarry -4520inputcarry_p vdd pti
xnti3 -4520inputcarry -4520inputcarry_n vdd nti

xckt0 -4520inputb -4520inputb_p -4520inputb_n -4520inputcarry -4520inputcarry_p -4520inputcarry_n -157332PortB vdd f_20K


xckt1 -4520inputb -4520inputb_p -4520inputb_n -4520inputcarry -4520inputcarry_p -4520inputcarry_n -157332PortB vdd f_20K


xckt2 -4520inputb -4520inputb_p -4520inputb_n -4520inputcarry -4520inputcarry_p -4520inputcarry_n -157332PortB vdd f_K00


xckt3 -4520inputb -4520inputb_p -4520inputb_n -4520inputcarry -4520inputcarry_p -4520inputcarry_n -157332PortB vdd f_K00


xckt4 -4520inputb -4520inputb_p -4520inputb_n -4520inputcarry -4520inputcarry_p -4520inputcarry_n -157332PortB vdd f_ZKK



.ends


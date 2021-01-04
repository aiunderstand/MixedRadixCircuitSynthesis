.subckt circuit in0 in1 out0 out1 vdd
.include "f_20K.sp"
.include "f_K00.sp"
.include "f_ZKK.sp"
.include "nti.sp" 
.include "pti.sp"

xpti0 in0 in0_p vdd pti
xpti1 in1 in1_p vdd pti

xckt0 in0 in0_p in1 in1_p out_ckt0 vdd f_20K

xpti2 out_ckt0 out_ckt0_p vdd pti
xpti3 in2 in2_p vdd pti

xckt1 out_ckt0 out_ckt0_p in2 in2_p out0 vdd f_20K


xckt2 in0_p in1_p out_ckt2 vdd f_K00


xckt3 out_ckt0_p in2_p out_ckt3 vdd f_K00

xpti4 out_ckt2 out_ckt2_p vdd pti
xpti5 out_ckt3 out_ckt3_p vdd pti

xckt4 out_ckt2_p out_ckt3_p out2 vdd f_ZKK



.ends


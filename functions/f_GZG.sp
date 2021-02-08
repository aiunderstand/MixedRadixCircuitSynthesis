.subckt f_GZG i0 i0_n i1 i1_n out vdd


xp0 up out out gnd PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 13 
xn1 out out down gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 13 


***pullup full


xp2 vdd i0 p0 gnd PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19  
xp3 p0 i0_n out gnd PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19  

xp4 vdd i1 p1 gnd PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19  
xp5 p1 i1_n out gnd PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19  

***pulldown full


***pullup half


***pulldown half


xn6 gnd i0_n p2 gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19 
xn7 p2 i1_n down gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19 

xn8 gnd i0 p3 gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 10 
xn9 p3 i1_n down gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19 

xn10 gnd i0_n p4 gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19 
xn11 p4 i1 down gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 10 

xn12 gnd i0 p5 gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 10 
xn13 p5 i1 down gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 10 

.ends


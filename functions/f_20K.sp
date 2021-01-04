.subckt f_20K i0 i0_p i1 i1_p out vdd


xp0 up out out gnd PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 13 
xn1 out out down gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 13 


***pullup full


xp2 vdd i0_p p0 gnd PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19  
xp3 p0 i1 out gnd PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 10  

xp4 vdd i0 p1 gnd PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 10  
xp5 p1 i1_p out gnd PCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbp = 'Vfp' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19  

***pulldown full


xn6 gnd i0_p p2 gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19 
xn7 p2 i1_p out gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19 

xn8 gnd i0 p3 gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19 
xn9 p3 i1 out gnd NCNFET Lch=Lg  Lgeff='Lgef' Lss=32e-9  Ldd=32e-9 
+Kgate = 'Kox' Tox = 'Hox' Csub = 'Cb' Vfbn = 'Vfn' Dout = 0  Sout = 0  Pitch = 20e-9 tubes = 3  n2 = n  n1 = 19 

***pullup half


***pulldown half


.ends


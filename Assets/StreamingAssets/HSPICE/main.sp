ternary circuits

.options POST
.options AUTOSTOP
.options INGOLD=2     DCON=1
.options GSHUNT=1e-12 RMIN=1e-15 
.options ABSTOL=1N  ABSVDC=1e-4 
.options RELTOL=1e-2  RELVDC=1e-2 
.options NUMDGT=4     PIVOT=13
.options VNTOL=1M
.OPTION CONVERGE=5
.options dcstep = 100
.OPTIONS METHOD=GEAR
.param TEMP = 25
.options runlvl=0

.lib 'CNFET.lib' CNFET 

*Some CNFET parameters:
.param Ccsd=0      CoupleRatio=0
.param m_cnt=1     Efo=0.6     
.param Wg=0        Cb=40e-12
.param Lg=32e-9    Lgef=100e-9
.param Vfn=0       Vfp=0
.param m=19        n=0        
.param Hox=4e-9    Kox=16 

Vd     	top     Gnd     0.9
Vm		top		vdd		0

***input pattern
Vin0 in0 gnd PWL(0ps 0.00v 249ps 0.00v 250ps 0.90v 499ps 0.90v 500ps 0.00v 749ps 0.00v 750ps 0.90v 999ps 0.90v 1000ps 0.00v 1249ps 0.00v 1250ps 0.90v 1499ps 0.90v 1500ps 0.00v 1749ps 0.00v 1750ps 0.90v 1999ps 0.90v 2000ps 0.00v 2375ps 0.00v)

Vin1 in1 gnd PWL(0ps 0.45v 374ps 0.45v 375ps 0.0v 624ps 0.0v 625ps 0.45v 874ps 0.45v 875ps 0.90v 1124ps 0.90v 1125ps 0.45v 1374ps 0.45v 1375ps 0.45v 1624ps 0.45v 1625ps 0.45v 1874ps 0.45v 1875ps 0.00v 2124ps 0.00v 2125ps 0.00v 2375ps 0.00v)


***main circuit
.include 'c_CIRCUITNAME.sp' 
xTFA in0 in1 result vdd c_CIRCUITNAME 

***********************************************************************
* Measurements
***********************************************************************

.measure tran iavgsum avg i(vm) from=0p to=2375ps

.tran 10p 2375ps

.print V(in0)
.print V(in1)

.print V(result)

.end 





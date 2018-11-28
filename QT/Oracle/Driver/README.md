### Driver Compile

Edit setup file : Windows 10 + MSVC

```
oci.pro
qsqldriverbase.pri
```

make

```
oci.dll etc...
set PATH=%PATH%;C:\instantclient_12_2 
qmake -- OCI_INCDIR=C:\instantclient_12_2\sdk\include OCI_LIBDIR=C:\instantclient_12_2\sdk\lib\msvc oci.pro
nmake
```

qsqloci.dll, qsqlocid.dll : Copy to ...\msvc2017_64\plugins\sqldrivers

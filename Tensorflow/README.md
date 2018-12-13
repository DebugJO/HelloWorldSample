visual studio 2017 : vcvars64.bat, cmake PATH 설정
download swigwin : http://www.swig.org/download.html

```
git clone https://github.com/tensorflow/tensorflow.git v1.6.0
cd v1.6.0
git checkout tags/v1.6.0
```

```
cd tensorflow\contrib\cmake
mkdir build
cd build
```

```
cmake .. -A x64 ^
-DCMAKE_BUILD_TYPE=Release ^
-DSWIG_EXECUTABLE=:\폴더\swig.exe ^
-DPYTHON_EXECUTABLE=C:\폴더\Python36\python.exe ^
-DPYTHON_LIBRARIES=C:\폴더\Python36\libs\python35.lib ^
-Dtensorflow_BUILD_PYTHON_BINDINGS=OFF ^
-Dtensorflow_ENABLE_GRPC_SUPPORT=OFF ^
-Dtensorflow_BUILD_SHARED_LIB=ON
```

```
MSBuild ^
/m:1 ^
/p:CL_MPCount=1 ^
/p:Configuration=Release ^
/p:Platform=x64 ^
/p:PreferredToolArchitecture=x64 ALL_BUILD.vcxproj ^
/filelogger
```

최종적으로 Release 폴더에 tensorflow.dll, tensorflow.lib 파일이 생성됨

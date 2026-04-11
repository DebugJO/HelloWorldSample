# 프로젝트 폴더 설명

### csProject : C\# 전체소스

* ConsoleApp : exe managed
* ConsoleAppLib : dll managed(LogHelper.cs)
* ConsoleAppNative : dll unmanaged aot

CppNative.dll, rust_native.dll, zig_native.dll 파일은 ConsoleApp 폴더에 복사 후 사용. 즉, ConsoleApp.csproj 파일이 있는 곳.

### CppNative : C\++ 전체소스

* CMakeLists.txt
* library.cpp
* library.h

### rust_native : rust 전체소스

* Cargo.toml
* .cargo/config.toml
* src/lib.rs

### zig_native : zig 전체소스

* build.zig
* src/main.zig


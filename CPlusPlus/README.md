### msys2 ucrt64

```bash
pacman -Syu

pacman -S --needed \
mingw-w64-ucrt-x86_64-gcc \
mingw-w64-ucrt-x86_64-clang \
mingw-w64-ucrt-x86_64-cmake \
mingw-w64-ucrt-x86_64-ninja \
mingw-w64-ucrt-x86_64-gdb \
mingw-w64-ucrt-x86_64-lldb \
mingw-w64-ucrt-x86_64-pkgconf \
mingw-w64-ucrt-x86_64-python \
mingw-w64-ucrt-x86_64-meson \
git \
make

// 추가
pacman -S --needed \
mingw-w64-ucrt-x86_64-clang-tools-extra \
mingw-w64-ucrt-x86_64-ccache

// clang-tools-extra 에 다음 3개 포함
clangd
clang-tidy
clang-format

// qt 기본
pacman -S --needed \
mingw-w64-ucrt-x86_64-qt6-base \
mingw-w64-ucrt-x86_64-qt6-tools

// qt ui 개발 추가
pacman -S --needed \
mingw-w64-ucrt-x86_64-qt6-svg \
mingw-w64-ucrt-x86_64-qt6-imageformats \
mingw-w64-ucrt-x86_64-qt6-5compat

// qt 추가 옵션
pacman -S --needed \
mingw-w64-ucrt-x86_64-qt6-multimedia
qt6-webengine // 용량 큼 주의(필요할 때만 설치)


// 검색
pacman -Ss qt6
pacman -Ss ucrt

// 설치된 패키지 확인
pacman -Qs qt6

// 의존성
pactree mingw-w64-ucrt-x86_64-qt6-base

pacman -S --needed \
mingw-w64-ucrt-x86_64-ntldd

ntldd myapp.exe
objdump -p myapp.exe | grep DLL
llvm-objdump -p myapp.exe
```

### 기본 소스 헤더 분리 예제

```cpp
// main.cpp
#include "helloworld.h"

using namespace std;

int main()
{
    HelloWorld hello;
    string name = "가나닭";
    hello.SetName(name);
    hello.GetName();
}

// header : helloworld.h
#ifndef HELLOWORLD_H
#define HELLOWORLD_H

#include <string>

using namespace std;

class HelloWorld
{
  public:
    HelloWorld();
    ~HelloWorld();

    void SetName(const string name);
    void GetName() const;

  private:
    string name;
};

#endif // HELLOWORLD_H

// source(cpp) : helloworld.cpp
#include "helloworld.h"
#include <iostream>
#include <string>

using namespace std;

HelloWorld::HelloWorld()
{
    cout << "Hello World!" << endl;
}

HelloWorld::~HelloWorld()
{
    cout << "Goodbye..." << endl;
}

void HelloWorld::SetName(const string name)
{
    this->name = name;
}

void HelloWorld::GetName() const
{
    cout << "Hello World! : " + this->name << endl;
}

// Jekyll 에 Bundler 사용하기
bundle install
bundle exec jekyll serve
bundle exec jekyll serve --livereload
```

### char(기본자료형), string(클래스) 비교

```cpp
#include <iostream>
#include <cstring>

using namespace std;

int main()
{
    char arr[100] = {'H', 'e', 'l', 'l', 'o'};
    cout << sizeof(arr) << " : " << arr << endl;

    char arr1[] = "Hello";
    cout << sizeof(arr1) << " : " << arr1 << endl;

    const char *arr2 = "Hello";
    cout << sizeof(arr2) << " : " << arr2 << endl;

    char *arr3 = new char[100];
    strcpy_s(arr3, 100, "Hello");
    strcat_s(arr3, 100, " World");
    cout << strlen(arr3) << " : " << arr3 << endl;

    delete[] arr3;

    string str = "Hello";
    str = str + " World";
    cout << size(str) << " : " << str << endl;

    const char *strChar = str.c_str();
    cout << strlen(strChar) << " : " << strChar << endl;

    return 0;
}
```

### CLion 콘솔 환경 설정(Windows)
* 설정에서 콘솔, 파일 인코딩 부분을 모두 UTF-8로 바꾼다
* Cmakelist.txt 파일을 다음과 같이 설정(추가)
```txt
set(CMAKE_CXX_FLAGS "-fexec-charset=CP949")
```


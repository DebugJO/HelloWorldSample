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

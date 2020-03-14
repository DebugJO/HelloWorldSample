### QT(C++) Default Windows Console Template

```cpp
#include <iostream>
#ifdef WIN32
#include <windows.h>
#endif

using namespace std;

#pragma pack(1) // windows
struct Person {
    string name;
    int age;
    double salary;

    void Print() const;
    void ChageAge(int e);
}; //__attribute__((packed)); // linux

int main()
{
#ifdef WIN32
    SetConsoleOutputCP(CP_UTF8);
#endif

    Person a, b, c;
    a.name = "가나닭";
    a.age = 20;
    a.salary = 5000.1;
    b.name = "홍길동";
    b.age = 31;
    b.salary = 8000.2;
    c.name = "마바리";
    c.age = 50;
    c.salary = 4000.3;
    a.ChageAge(100);
    a.Print();
    b.Print();
    c.Print();
    return 0;
}

void Person::Print() const
{
    cout << "이름: " << name << endl;
    cout << "나이: " << age << endl;
    cout << "월급: " << salary << endl;
    cout << endl;
}

void Person::ChageAge(int e)
{
    age = e;
}
```

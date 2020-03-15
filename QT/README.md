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

### C++ Interface

```cpp
#include <iostream>
#ifdef WIN32
#include <windows.h>
#endif

using namespace std;

class Person {
  public:
    virtual ~Person();
    virtual void Speak(const string &words) const = 0;
    virtual void Walk(int km) const = 0;
    virtual void Work(int hr) = 0;
    virtual void Play(int hr) const = 0;

    double GetSalary() const;

  protected:
    double salary;
};

Person::~Person() { cout << "...END..." << endl; }

double Person::GetSalary() const { return salary; }

class People : public Person {
  public:
    People();

    void Speak(const string &words) const override;

    void Walk(int km) const override;

    void Work(int hr) override;

    void Play(int hr) const override;
};

People::People() { salary = 0.0; }

void People::Speak(const string &words) const { cout << words << endl; }

void People::Walk(int km) const { cout << "People walked " << km << endl; }

void People::Work(int hr) { salary = hr * 20.0; }

void People::Play(int hr) const { cout << "People Played " << hr << " hours" << endl; }

int main()
{
#ifdef WIN32
    SetConsoleOutputCP(CP_UTF8);
#endif

    People p;
    p.Speak("헬로우월드");
    p.Walk(20);
    p.Work(8);
    p.Play(3);
    cout << p.GetSalary() << endl;

    return 0;
}
```

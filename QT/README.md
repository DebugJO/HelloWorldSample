### Qt/C\+\+ 포인터, 레퍼런트, 베열 및 문자열 처리

##### 

```cpp
#include <QCoreApplication>
#include <QDebug>
#include <fcntl.h>
#include <iostream>

using namespace std;

unsigned int ToINT(QString StrNumber, bool &ok);

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    int iNum = 10;
    int *pNum = &iNum;
    int &rNum = iNum;

    cout << "--- pointer ---" << endl;
    cout << *pNum << endl; // 10
    cout << pNum << endl;  // 0x...
    cout << &iNum << endl; // 0x...
    cout << "--- reference ---" << endl;
    cout << rNum << endl;  // 10
    cout << &rNum << endl; // 0x...
    cout << "-----------------" << endl;

    _setmode(_fileno(stdout), _O_U16TEXT);

    QString snum = "12345";
    bool ok = false;
    unsigned int num = ToINT(snum, ok);
    if (ok)
        qDebug() << QLocale::system().toString(num).toStdString().c_str(); // 12,345
    else
        qDebug() << "숫자가 아닙니다.";

    QString qstr = "헬로우월드";
    qDebug() << qstr;                                              // "헬로우월드"
    qDebug() << QString(QChar(qstr[0])).toStdString().c_str();     //헬
    qDebug() << QString(QChar(qstr[1])) + QString(QChar(qstr[2])); // "로우"

    wchar_t *ustr = new wchar_t[qstr.length()];
    qstr.toWCharArray(ustr);
    wcout << ustr[3] << ustr[4]; //월드
    wcout << "" << endl;

    int const size = wcslen(ustr);
    for (int i = 0; i < size; ++i) {
        qDebug() << QString(QChar(ustr[i])).toStdString().c_str(); //헬 로 우 월 드
    }

    qDebug() << QString::fromWCharArray(ustr).toStdString().c_str(); //헬로우월드

    return a.exec();
}

unsigned int ToINT(QString StrNumber, bool &ok)
{
    unsigned int num = StrNumber.toUInt(&ok);
    if (ok)
        return num;
    else
        return 0;
}
```

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

Person::~Person() { cout << "...Person Class END..." << endl; }

double Person::GetSalary() const { return salary; }

class People : public Person {
  public:
    People();
    ~People() override;

    void Speak(const string &words) const override;

    void Walk(int km) const override;

    void Work(int hr) override;

    void Play(int hr) const override;
};

People::People() { salary = 0.0; }

People::~People() { cout << "...People Class END..." << endl; }

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

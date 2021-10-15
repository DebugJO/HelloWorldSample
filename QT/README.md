### CMake, Windows codepage cp949 설정
```
set(CMAKE_CXX_FLAGS "-fexec-charset=CP949")
```

### Ubuntu(Linux)에서 QtCreator 실행에 필요한 추가 패키지
```
sudo apt-get install libxcb*
sudo apt-get install mesa-common-dev libgl1-mesa-dev libglu1-mesa-dev
```

### WIN32 파일을 실행할 때 콘솔도 같이 실행될 때
1. cmake file 수정(추가) : set_target_properties(untitled qt 6.2 cmake PROPERTIES WIN32_EXECUTABLE TRUE ...
2. 메인소스 상단에 코드 추가
```cpp
#ifdef _WIN32
#pragma comment(linker, "/SUBSYSTEM:windows /ENTRY:mainCRTStartup")
#endif
```

### C\+\+ 포인터와 레퍼런스 차이

1. 포인터 : 메모리의 주소를 가지고 있는 변수, 주소 값을 통한 메모리 접근(간접 참조)
2. 레퍼런스 : 자신이 참조하는 변수를 대신할 수 있는 또 하나의 이름(별칭추가), 변수 명을 통해서 메모리 참조(직접 참조)
3. 포인터/레퍼런스 차이 (https://ssinyoung.tistory.com/16)
    1. NULL 초기화 : 포인터는 NULL(nullptr) 초기화를 할 수 있지만 레퍼런스는 반드시 선언과 동시에 초기화 해야 한다.
    2. 포인터는 주소 값을 저장하기 위해 별도의 메모리 공간을 소모, 레퍼런스는 같은 메모리 공간을 참조하므로 메모리 공간을 소모하지 않는다.
    3. 매개 변수로 함수 인자 전달 : call by pointer 메모리 소모가 일어나고, 값 복사가 발생 / call by reference 메모리 소모가 없고, 값 복사 발생하지 않는다.

```cpp
int iNum = 10;
int *pNum = nullptr;
int &rNum = nullptr; //에러

void SwapPointer(int *pNum1, int *pNum2)
{
    int iTemp = *pNum1;
    *pNum1 = *pNum2;
    *pNum2 = iTemp;
}

void SwapReference(int &rNum1, int &rNum2)
{
    int iTemp = rNum1;
    rNumm = rNum2;
    rNum2 = iTemp;
}
```

### Qt/C\+\+ 포인터, 레퍼런스, 베열 및 문자열 처리

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

### 스마트 포인터(Smart Pointer)

```cpp
#include <iostream>
#include <memory>
#include <windows.h>

using namespace std;

class Person
{
  private:
    string mName;
    int mAge;
    static int cCount;
    static int dCount;

  public:
    Person(const string &name, int age);
    ~Person()
    {
        dCount++;
        cout << "소멸자가 호출되었습니다 : " << dCount << ":" << mAge << endl;
    }
    void ShowPersonInfo();
};

Person::Person(const string &name, int age)
{
    mName = name;
    mAge = age;
    cCount++;
    cout << "생성자가 호출되었습니다 : " << cCount << ":" << mAge << endl;
}

void Person::ShowPersonInfo()
{
    cout << mName << "의 나이는 " << mAge << "살입니다." << endl;
}

int Person::cCount = 0;
int Person::dCount = 0;

int main(void)
{
#ifdef WIN32
    SetConsoleOutputCP(CP_UTF8);
#endif
    // unique_ptr<Person> hong = make_unique<Person>("길동", 29);
    auto hong1 = make_unique<Person>("길동", 29);
    hong1->ShowPersonInfo();

    // hong.reset();

    auto hong2 = make_unique<Person>("길서", 30);
    hong2->ShowPersonInfo();

    return 0;
}
```

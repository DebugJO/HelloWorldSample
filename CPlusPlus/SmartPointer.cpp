#include <iostream>
#include <memory>

using namespace std;

class Person
{
  private:
    string name_;
    int age_;

  public:
    Person(const string &name, int age);
    ~Person() { cout << "소멸자가 호출되었습니다." << endl; }
    void ShowPersonInfo();
};

Person::Person(const string &name, int age)
{
    name_ = name;
    age_ = age;
    cout << "생성자가 호출되었습니다." << endl;
}

void Person::ShowPersonInfo() { cout << name_ << "의 나이는 " << age_ << "살입니다." << endl; }

int main(void)
{
    // unique_ptr<Person> hong = make_unique<Person>("길동", 29);
    auto hong = make_unique<Person>("길동", 29);
    hong->ShowPersonInfo();
    //hong.reset();
    return 0;
}

#include <QCoreApplication>
#include <QDebug>
#include <iostream>
#include <memory>

using namespace std;

class Person : public QObject
{
  public:
    explicit Person(QObject *parent = nullptr) : QObject(parent)
    {
    }

    ~Person()
    {
        qDebug() << ".........End :" << mName;
    }

    void setName(const QString &name)
    {
        mName = name;
    }

    void setAge(int age)
    {
        mAge = age;
    }

    void setSalary(double salary)
    {
        mSalary = salary;
    }

    QString getName() const
    {
        return mName;
    }

    int getAge()
    {
        return mAge;
    }

    double getSalary()
    {
        return mSalary;
    }

    void Print() const
    {
        qDebug() << mName << mAge << mSalary;
    }

  private:
    QString mName;
    int mAge;
    double mSalary;
};

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    auto aaa = make_unique<Person>();
    auto bbb = make_unique<Person>(&(*aaa));
    auto ccc = make_unique<Person>(addressof(*aaa));

    aaa->setName("홍길동");
    aaa->setAge(30);
    aaa->setSalary(3000.30);

    bbb->setName("홍길서");
    bbb->setAge(40);
    bbb->setSalary(4000.40);

    ccc->setName("홍길남");
    ccc->setAge(50);
    ccc->setSalary(5000.50);

    aaa->Print();
    bbb->Print();
    ccc->Print();

    a.exit(0);
}

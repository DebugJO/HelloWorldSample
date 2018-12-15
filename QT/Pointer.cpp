#include <QCoreApplication>
#include <QDebug>
#include <QPointer>
#include <iostream>
#include <memory>

using namespace std;


class Person : public QObject
{
public:
    explicit Person(const QString &name, QObject *parent = nullptr);
    ~Person();
    virtual QString name() const;

private:
    QString m_name;
};

Person::Person(const QString &name, QObject *parent) : QObject(parent)
{
    m_name = name;
}

Person::~Person()
{
    qDebug() << "...End...";
}

QString Person::name() const
{
    return m_name;
}

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    QPointer<Person> ptr1 = new Person("DEBUG1");
    Person *ptr2 = new Person("DEBUG2");
    auto ptr3 = make_unique<Person>("DEBUG3");

    qDebug() << ptr1->name();
    qDebug() << ptr2->name();
    qDebug() << ptr3->name();

    delete ptr1;
    delete ptr2;
    ptr3.reset();

    qDebug() << (ptr1 == nullptr);
    qDebug() << (ptr2 == nullptr);
    qDebug() << (ptr3 == nullptr);

    return a.exec();
}

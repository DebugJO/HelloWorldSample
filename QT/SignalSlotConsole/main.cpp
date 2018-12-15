#include <QCoreApplication>
#include "person.h"

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    QObject *p = new QObject;
    Person *p1 = new Person(p);
    Person *p2 = new Person(p);

    p1->setName("AAA");
    p2->setName("BBB");

    QObject::connect(p1, SIGNAL(speak(QString)), p2, SLOT(listen(QString)));
    QObject::connect(p2, SIGNAL(speak(QString)), p1, SLOT(listen(QString)));

    p1->speaks("aaaaaaaa");
    p2->speaks("bbbbbbbb");

    delete p;

    return a.exec();
}

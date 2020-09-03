#include "myclass.h"
#include <QCoreApplication>
#include <QScopedPointer>
#include <QSharedPointer>

void addItems(QList<QSharedPointer<MyClass>> &list)
{
    for (int i = 0; i < 10; i++) {
        MyClass *myclass = new MyClass(nullptr);
        myclass->setObjectName("MyClass" + QString::number(i));
        list.append(QSharedPointer<MyClass>(myclass));
    }
}

void testList()
{
    QList<QSharedPointer<MyClass>> list;
    addItems(list);
    qDebug() << "Count:" << list.length();
    list.removeAt(2);
    qDebug() << "Count:" << list.length();

    foreach (QSharedPointer<MyClass> item, list) {
        qDebug() << item->objectName();
    }
}

void test1()
{
    auto obj = QSharedPointer<MyClass>(new MyClass(nullptr), &QObject::deleteLater);
    obj->setObjectName("Test1");
}

void test2()
{
    auto obj = QScopedPointer<MyClass>(new MyClass(nullptr));
    obj->setObjectName("Test2");
}

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    testList();
    qDebug() << "-------------------------";
    test1();
    test2();

    return a.exec();
}

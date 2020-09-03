#include "myclass.h"

MyClass::MyClass(QObject *parent) : QObject(parent)
{
    qDebug() << this << "Constructed";
}

MyClass::~MyClass()
{
    qDebug() << this << "Destroyed";
}

#include "person.h"
#include <QDebug>

Person::Person(QObject *parent) : QObject(parent)
{
    qDebug() << ".....Start";
}

Person::~Person()
{
    qDebug() << ".....End";
}

void Person::speaks(const QString &words)
{
    emit speak(words);
}

void Person::listen(const QString &words)
{
    qDebug() << m_name << " has heard " << words;
}

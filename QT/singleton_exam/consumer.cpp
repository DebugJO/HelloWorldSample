#include "consumer.h"

Consumer::Consumer(QObject *parent) : QObject(parent)
{
    connect(Tester::Instance(), SIGNAL(TestSignal()), this, SLOT(TestSlot()));
}

void Consumer::TestSlot()
{
    qDebug() << "It's working!!!";
}

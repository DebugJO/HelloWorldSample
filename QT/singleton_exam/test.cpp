#include "test.h"
#include <QDebug>

Test::Test(QObject *parent) : QObject(parent){}

void Test::DoSomeThing()
{
    // emit TestSignal();
    Tester::Instance()->TestSignal();
}

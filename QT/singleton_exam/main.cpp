#include "consumer.h"
#include "test.h"
#include <QCoreApplication>

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    Consumer consumer;
    Tester::Instance()->DoSomeThing();
    // Test::DoSomeThing();

    a.exit(0);
}

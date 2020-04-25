#ifndef TEST_H
#define TEST_H

#include "singleton.h"
#include <QObject>

class Test : public QObject
{
    Q_OBJECT
  public:
    explicit Test(QObject *parent = nullptr);

    static void DoSomeThing();

  signals:
    void TestSignal();
};

// Global variable
typedef Singleton<Test> Tester;

#endif // TEST_H

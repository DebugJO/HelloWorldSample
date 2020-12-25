#include <QCoreApplication>
#include <QDebug>
#include <exception>

using namespace std;

class Cat
{
  public:
    Cat()
    {
        qInfo() << "Call constructor";
    }

    ~Cat()
    {
        qInfo() << "Call destructor";
    }

    int GetAge()
    {
        return mAge;
    }

  private:
    int mAge = 10;
};

int divide(int a, int b)
{
    if (b == 0) {
        throw runtime_error("divide by 0");
    }
    return a / b;
}

void f()
{
    auto cp = make_unique<Cat>();
    qInfo() << cp->GetAge();
    qInfo() << divide(10, 0);
}

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    try {
        f();
    } catch (exception &ex) {
        qInfo() << ex.what();
    }

    return a.exec();
}

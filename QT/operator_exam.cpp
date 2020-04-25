#include <QCoreApplication>
#include <QDebug>

class Person
{
  public:
    Person(const QString &name, int age) : mName(name), mAge(age)
    {
    }
    friend inline QDebug operator<<(QDebug qd, const Person &p);

  private:
    QString mName;
    int mAge;
};

inline QDebug operator<<(QDebug qd, const Person &p)
{
    return qd << p.mName << " " << p.mAge;
}

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    Person aaa("AAA", 29);
    qDebug() << aaa;

    a.exit(0);
    // return a.exec();
}

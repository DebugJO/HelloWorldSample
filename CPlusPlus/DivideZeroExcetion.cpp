#include <QCoreApplication>
#include <QDebug>
#include <csignal>
#include <cstdlib>
#include <iostream>

using namespace std;

void fpe_handler(int signal)
{
    qInfo() << "Floating Point Exception: division by zero";
    exit(signal);
}

qint32 Add(qint32 i, qint32 j, QString &err)
{
    if (j == 0) {
        err = "division by zero";
        return 0;
    }
    return i / j;
}

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    signal(SIGFPE, fpe_handler);

    QString err = "정상처리";
    qInfo() << "나누기:" << Add(1, 0, err) << ":" << err.toStdString().c_str();

    return a.exec();
}

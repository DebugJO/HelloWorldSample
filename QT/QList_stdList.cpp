#include <QCoreApplication>
#include <QDebug>
#include <iostream>

using namespace std;

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    list<string> s;
    list<string>::iterator itr;

    QList<QString> S;

    S << "가나닭"
      << "ABC";
    S.append("123");

    for (int i = 0; i < S.size(); ++i) {
        qDebug() << S[i];
        s.push_back(S[i].toStdString().c_str());
    }

    for (itr = s.begin(); itr != s.end(); ++itr) {
        qDebug() << QString::fromStdString(*itr);
    }

    a.exit(0);
}

#include <QCoreApplication>
#include <QDebug>
#include <QString>

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    QString str = "123가나닭";
    QString res = str.toLocal8Bit().toHex().toUpper();

    qDebug() << "1:" << res;
    qDebug() << "2:" << res.toStdString().c_str();

    QByteArray hex = QByteArray::fromHex(res.toStdString().c_str());
    QString str2 = QString::fromLocal8Bit(hex.data());
    qDebug() << "3:" << str2;//.toStdString().c_str();

    return a.exec();
}

#include <QCoreApplication>
#include <QtSql/QSqlDatabase>
#include <QtSql/QSqlError>
#include <QDebug>

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

    QSqlDatabase db = QSqlDatabase::addDatabase("QOCI");

    db.setHostName("IP Address or Domain");
    db.setDatabaseName("orcl");
    db.setUserName("ID");
    db.setPassword("Password");

    if (!db.open())
    {
        qDebug() << db.lastError().text();
    }
    else{
        qDebug() << "Wow opened";
    }

    return a.exec();
}

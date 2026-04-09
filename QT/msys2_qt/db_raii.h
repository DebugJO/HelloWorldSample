#ifndef MSYS2_QT_DB_RAII_H
#define MSYS2_QT_DB_RAII_H

#include <sqlite3.h>
#include <QSqlDatabase>
#include <QSqlError>
#include <QDebug>
#include <iostream>
#include <string>
#include <utility>

class NativeSqliteRAII {
public:
    explicit NativeSqliteRAII(const std::string &dbPath) {
        if (sqlite3_open(dbPath.c_str(), &db) == SQLITE_OK) {
            std::cout << "[Native] DB Opened: " << dbPath << std::endl;
        } else {
            std::cerr << "[Native] Open Error: " << sqlite3_errmsg(db) << std::endl;
            db = nullptr;
        }
    }

    ~NativeSqliteRAII() {
        if (db) {
            sqlite3_close(db);
            std::cout << "[Native] DB Closed Automatically." << std::endl;
        }
    }

    [[nodiscard]] sqlite3 *handle() const { return db; }

private:
    sqlite3 *db = nullptr;
};


class QtSqlRAII {
public:
    QtSqlRAII(QString connName, const QString &dbName) : m_connName(std::move(connName)) {
        QSqlDatabase db = QSqlDatabase::addDatabase("QSQLITE", m_connName);
        db.setDatabaseName(dbName);

        if (db.open()) {
            qDebug() << "[Qt] DB Opened with connection:" << m_connName;
        } else {
            qDebug() << "[Qt] Open Error:" << db.lastError().text();
        }
    }

    ~QtSqlRAII() {
        {
            if (QSqlDatabase db = QSqlDatabase::database(m_connName); db.isOpen()) {
                db.close();
            }
        }

        QSqlDatabase::removeDatabase(m_connName);
        qDebug() << "[Qt] Connection" << m_connName << "Removed Automatically.";
    }

private:
    QString m_connName;
};

#endif //MSYS2_QT_DB_RAII_H

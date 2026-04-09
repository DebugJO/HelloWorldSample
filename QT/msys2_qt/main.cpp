#include <QCoreApplication>
#include "db_raii.h"

void testNative() {
    std::cout << "Entering testNative scope..." << std::endl;
    NativeSqliteRAII nativeDb(":memory:");
    std::cout << "Leaving testNative scope..." << std::endl;
}

void testQt() {
    qDebug() << "Entering testQt scope...";
    QtSqlRAII qtDb("TestConn", ":memory:");
    qDebug() << "Leaving testQt scope...";
    qDebug() << Q_FUNC_INFO;
}

int main(int argc, char *argv[]) {
    QCoreApplication a(argc, argv);
    std::cout << "=== Starting RAII Test ===" << std::endl;
    testNative();
    std::cout << "--------------------------" << std::endl;
    testQt();
    std::cout << "\n=== Test Finished ===" << std::endl;
    return 0;
}

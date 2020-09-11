#include "mainwindow.h"
#include <QApplication>

int main(int argc, char *argv[])
{
    int result = 0;
    do {
        QApplication a(argc, argv);
        MainWindow w;
        w.show();
        result = a.exec();
    } while (result == 33333);

    return result;
}

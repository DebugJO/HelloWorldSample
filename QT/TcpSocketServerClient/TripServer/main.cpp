#include <QApplication>
#include <QtGui>
#include <QPushButton>
#include <iostream>
#include "tripserver.h"

int main(int argc, char *argv[])
{
    QApplication app(argc, argv);

    TripServer server;
    if (!server.listen(QHostAddress::Any, 6178)) {
        std::cerr << "Failed to bind to port" << std::endl;
        return 1;
    }

    QPushButton quitButton(QObject::tr("&Quit"));
    quitButton.setWindowTitle(QObject::tr("Trip Server"));
    quitButton.setGeometry(500, 500, 300, 200);
    QObject::connect(&quitButton, SIGNAL(clicked()), &app, SLOT(quit()));
    quitButton.show();

    return app.exec();
}

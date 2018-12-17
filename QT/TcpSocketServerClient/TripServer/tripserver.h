#ifndef TRIPSERVER_H
#define TRIPSERVER_H

#include <QTcpServer>


class TripServer : public QTcpServer
{
    Q_OBJECT

public:
    TripServer(QObject *parent = nullptr);

private:
    void incomingConnection(qintptr socketId);
};

#endif // TRIPSERVER_H

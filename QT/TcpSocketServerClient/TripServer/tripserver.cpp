#include <QtCore>
#include "clientsocket.h"
#include "tripserver.h"

TripServer::TripServer(QObject *parent) : QTcpServer (parent)
{

}

void TripServer::incomingConnection(qintptr socketId)
{
    ClientSocket *socket = new ClientSocket(this);
    socket->setSocketDescriptor(socketId);
}

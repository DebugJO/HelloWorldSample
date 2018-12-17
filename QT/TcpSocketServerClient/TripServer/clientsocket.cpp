#include <QtNetwork>
#include "clientsocket.h"

ClientSocket::ClientSocket(QObject *parent) : QTcpSocket(parent)
{
    connect(this, SIGNAL(readyRead()), this, SLOT(readClient()));
    connect(this, SIGNAL(disconnected()), this, SLOT(deleteLater()));
    nextBlockSize = 0;
}

void ClientSocket::readClient()
{
    QDataStream in(this);
    in.setVersion(QDataStream::Qt_5_12);

    if (nextBlockSize == 0) {
        if (bytesAvailable() < static_cast<quint16>(sizeof(quint16)))
            return;
        in >> nextBlockSize;
    }

    if (bytesAvailable() < nextBlockSize)
        return;

    quint8 requestType;
    QString from;
    QString to;
    QDate date;
    QTime time;
    quint8 flag;

    in >> requestType;
    if (requestType == 'S') {
        in >> from >> to >> date >> time >> flag;

        std::srand(static_cast<uint>(from.length()) * 3600 + static_cast<uint>(to.length()) * 60 + static_cast<uint>(time.hour()));
        int numTrips = std::rand() % 8;
        for (int i = 0; i < numTrips; ++i)
            generateRandomTrip(from, to, date, time);

        QDataStream out(this);
        out << quint16(0xFFFF);
    }

    close();
}

void ClientSocket::generateRandomTrip(const QString &, const QString &, const QDate &date, const QTime &time)
{
    QByteArray block;
    QDataStream out(&block, QIODevice::WriteOnly);
    out.setVersion(QDataStream::Qt_5_12);
    quint16 duration = std::rand() % 200;
    out << quint16(0) << date << time << duration << quint8(1)
        << QString("InterCity");
    out.device()->seek(0);
    out << static_cast<quint16>(static_cast<quint16>(block.size()) - sizeof(quint16));
    write(block);
}

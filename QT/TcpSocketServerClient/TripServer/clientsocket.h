#ifndef CLIENTSOCKET_H
#define CLIENTSOCKET_H

#include <QTcpSocket>

class QDate;
class QTime;

class ClientSocket : public QTcpSocket
{
    Q_OBJECT

public:
    ClientSocket(QObject *parent = nullptr);

private slots:
    void readClient();

private:
    void generateRandomTrip(const QString &from, const QString &to,const QDate &date, const QTime &time);
    quint16 nextBlockSize;
};

#endif // CLIENTSOCKET_H

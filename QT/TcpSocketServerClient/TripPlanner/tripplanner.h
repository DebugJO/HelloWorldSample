#ifndef TRIPPLANNER_H
#define TRIPPLANNER_H

#include <QDialog>
#include <QTcpSocket>
#include <QPushButton>

#include "ui_tripplanner.h"

class TripPlanner : public QDialog//, private Ui::TripPlanner
{
    Q_OBJECT

public:
    TripPlanner(QWidget *parent = nullptr);

private slots:
    void connectToServer();
    void sendRequest();
    void updateTableWidget();
    void stopSearch();
    void connectionClosedByServer();
    void error();

private:
    void closeConnection();

    Ui::TripPlanner *ui;

    QPushButton *searchButton;
    QPushButton *stopButton;
    QTcpSocket tcpSocket;
    quint16 nextBlockSize;
};

#endif

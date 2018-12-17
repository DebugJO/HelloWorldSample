#include <QtGui>
#include <QtNetwork>

#include "tripplanner.h"

TripPlanner::TripPlanner(QWidget *parent) : QDialog(parent), ui(new Ui::TripPlanner)
{
    ui->setupUi(this);

    searchButton = new QPushButton();
    stopButton = new QPushButton();

    searchButton = ui->buttonBox->addButton(tr("&Search"), QDialogButtonBox::ActionRole);
    stopButton = ui->buttonBox->addButton(tr("S&top"), QDialogButtonBox::ActionRole);

    stopButton->setEnabled(false);
    ui->buttonBox->button(QDialogButtonBox::Close)->setText(tr("&Quit"));

    QDateTime dateTime = QDateTime::currentDateTime();
    ui->dateEdit->setDate(dateTime.date());
    ui->timeEdit->setTime(QTime(dateTime.time().hour(), 0));

    ui->progressBar->hide();
    ui->progressBar->setSizePolicy(QSizePolicy::Preferred, QSizePolicy::Ignored);

    ui->tableWidget->verticalHeader()->hide();
    ui->tableWidget->setEditTriggers(QAbstractItemView::NoEditTriggers);

    connect(searchButton, SIGNAL(clicked()),this, SLOT(connectToServer()));
    connect(stopButton, SIGNAL(clicked()), this, SLOT(stopSearch()));
    connect(ui->buttonBox, SIGNAL(rejected()), this, SLOT(reject()));

    connect(&tcpSocket, SIGNAL(connected()), this, SLOT(sendRequest()));
    connect(&tcpSocket, SIGNAL(disconnected()), this, SLOT(connectionClosedByServer()));
    connect(&tcpSocket, SIGNAL(readyRead()), this, SLOT(updateTableWidget()));
    connect(&tcpSocket, SIGNAL(error(QAbstractSocket::SocketError)), this, SLOT(error()));
}

void TripPlanner::connectToServer()
{
    tcpSocket.connectToHost(QHostAddress::LocalHost, 6178);
/*
#if 1
    tcpSocket.connectToHost(QHostAddress::LocalHost, 6178);
#else
    tcpSocket.connectToHost("localhost", 6178);
#endif
*/

    ui->tableWidget->setRowCount(0);
    searchButton->setEnabled(false);
    stopButton->setEnabled(true);
    ui->statusLabel->setText(tr("Connecting to server..."));
    ui->progressBar->show();

    nextBlockSize = 0;
}

void TripPlanner::sendRequest()
{
    QByteArray block;
    QDataStream out(&block, QIODevice::WriteOnly);
    out.setVersion(QDataStream::Qt_5_12);
    out << quint16(0) << quint8('S')
        << ui->fromComboBox->currentText()
        << ui->toComboBox->currentText()
        << ui->dateEdit->date()
        << ui->timeEdit->time();

    if (ui->departureRadioButton->isChecked()) {
        out << quint8('D');
    } else {
        out << quint8('A');
    }
    out.device()->seek(0);
    out << static_cast<quint16>(static_cast<quint16>(block.size()) - sizeof(quint16));
    tcpSocket.write(block);

    ui->statusLabel->setText(tr("Sending request..."));
}

void TripPlanner::updateTableWidget()
{
    QDataStream in(&tcpSocket);
    in.setVersion(QDataStream::Qt_5_12);

    forever {
        int row = ui->tableWidget->rowCount();

        if (nextBlockSize == 0) {
            if (tcpSocket.bytesAvailable() < static_cast<quint16>(sizeof(quint16)))
                break;
            in >> nextBlockSize;
        }

        if (nextBlockSize == 0xFFFF) {
            closeConnection();
            ui->statusLabel->setText(tr("Found %1 trip(s)").arg(row));
            break;
        }

        if (tcpSocket.bytesAvailable() < nextBlockSize)
            break;

        QDate date;
        QTime departureTime;
        QTime arrivalTime;
        quint16 duration;
        quint8 changes;
        QString trainType;

        in >> date >> departureTime >> duration >> changes >> trainType;
        arrivalTime = departureTime.addSecs(duration * 60);

        ui->tableWidget->setRowCount(row + 1);

        QStringList fields;
        fields << date.toString(Qt::LocalDate)
               << departureTime.toString(tr("hh:mm"))
               << arrivalTime.toString(tr("hh:mm"))
               << tr("%1 hr %2 min").arg(duration / 60)
                  .arg(duration % 60)
               << QString::number(changes)
               << trainType;
        for (int i = 0; i < fields.count(); ++i)
            ui->tableWidget->setItem(row, i,new QTableWidgetItem(fields[i]));
        nextBlockSize = 0;
    }
}

void TripPlanner::stopSearch()
{
    ui->statusLabel->setText(tr("Search stopped"));
    closeConnection();
}

void TripPlanner::connectionClosedByServer()
{
    if (nextBlockSize != 0xFFFF)
        ui->statusLabel->setText(tr("Error: Connection closed by server"));
    closeConnection();
}

void TripPlanner::error()
{
    ui->statusLabel->setText(tcpSocket.errorString());
    closeConnection();
}

void TripPlanner::closeConnection()
{
    tcpSocket.close();
    searchButton->setEnabled(true);
    stopButton->setEnabled(false);
    ui->progressBar->hide();
}

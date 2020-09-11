#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QDebug>

MainWindow::MainWindow(QWidget *parent) : QMainWindow(parent), ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    setWindowFlags(Qt::WindowMinMaxButtonsHint); // Qt::WindowTitleHint, Qt::CustomizeWindowHint
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_pushButton_clicked()
{
    auto msgButton = QMessageBox::warning(this, "프로그램 종료", "프로그램을 종료하시겠습니까?<br/>프로그램 종료대신 재실행을 원하면 [Cancel]을 클릭하세요.",
                                          QMessageBox::Yes | QMessageBox::No | QMessageBox::Cancel, QMessageBox::No);

    if (msgButton == QMessageBox::Yes) {
        close();
    } else if (msgButton == QMessageBox::Cancel) {
        qApp->exit(33333);
        qDebug() << "1111";
        close();
        qDebug() << "2222";
    }
}

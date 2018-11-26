#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QMessageBox>

MainWindow::MainWindow(QWidget *parent) : QMainWindow(parent), ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    connect(ui->pushButton, SIGNAL(clicked()), this, SLOT(SetLabelChange()));
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::SetLabelChange()
{
    QString tmp = QString::fromLocal8Bit("한글");
    ui->label->setText(tmp);
    QMessageBox::information(this, toKor("정보"), toKor("프로그램을 종료합니다"), toKor("예"));
    this->close();
}

#include "mainwindow.h"
#include "ui_mainwindow.h"
#include <QDebug>

MainWindow::MainWindow(QWidget *parent) : QMainWindow(parent), ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    //    fm01 = new Form01(nullptr);
    //    fm02 = new Form02(nullptr);

    //    fm01->setGeometry(0, 0, ui->frame->geometry().width(), ui->frame->geometry().height());
    //    fm02->setGeometry(0, 0, ui->frame->geometry().width(), ui->frame->geometry().height());

    //this->showFullScreen();

}

MainWindow::~MainWindow()
{
    if ((findChild<QWidget*>("Form01")) != nullptr)
    {
        qDebug() << fm01 << "Closed";
        delete fm01;
    }

    if ((findChild<QWidget*>("Form02")) != nullptr)
    {
        qDebug() << fm02 << "Closed";
        delete fm02;
    }

    qDebug() << this << "Closed";

    delete ui;
}

void MainWindow::on_pushButton1_clicked()
{
    if ((findChild<QWidget*>("Form02")) != nullptr)
        fm02->hide();

    if ((findChild<QWidget*>("Form01")) == nullptr)
        fm01 = new Form01();
    fm01->setParent(ui->frame);
    fm01->setGeometry(0, 0, ui->frame->geometry().width(), ui->frame->geometry().height());
    fm01->show();
    fm01->activateWindow();
    fm01->addText("Hello World");
    ui->pushButton1->setText(fm01->getText().left(16));
}

void MainWindow::on_pushButton2_clicked()
{
    /*
    while(QWidget* w = findChild<QWidget*>("Form01"))
    {
        delete w;
    }
    */

    if ((findChild<QWidget*>("Form01")) != nullptr)
        fm01->hide();

    if ((findChild<QWidget*>("Form02")) == nullptr)
        fm02 = new Form02();
    fm02->setParent(ui->frame);
    fm02->setGeometry(0, 0, ui->frame->geometry().width(), ui->frame->geometry().height());
    fm02->show();
    fm02->activateWindow();
}

void MainWindow::resizeEvent(QResizeEvent *event)
{
    resizeFrame();
    QWidget::resizeEvent(event);
}

void MainWindow::resizeFrame()
{
    if ((findChild<QWidget*>("Form01")) != nullptr)
        fm01->setGeometry(0, 0, ui->frame->geometry().width(), ui->frame->geometry().height());

    if ((findChild<QWidget*>("Form02")) != nullptr)
        fm02->setGeometry(0, 0, ui->frame->geometry().width(), ui->frame->geometry().height());
}

void MainWindow::on_pushButton_clicked()
{
    this->close();
}

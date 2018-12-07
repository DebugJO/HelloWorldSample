#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include "form01.h"
#include "form02.h"

namespace Ui {
class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT

public:
    explicit MainWindow(QWidget *parent = nullptr);
    ~MainWindow();

private slots:
    void on_pushButton1_clicked();
    void on_pushButton2_clicked();

    void on_pushButton_clicked();

private:
    Ui::MainWindow *ui;
    Form01 *fm01;
    Form02 *fm02;

protected:
    void resizeEvent(QResizeEvent *event);
    void resizeFrame();
};
#endif // MAINWINDOW_H

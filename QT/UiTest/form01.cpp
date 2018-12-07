#include "form01.h"
#include "ui_form01.h"

Form01::Form01(QWidget *parent) : QWidget(parent), ui(new Ui::Form01)
{
    ui->setupUi(this);
    setWindowFlag(Qt::WindowStaysOnTopHint);
}

Form01::~Form01()
{
    delete ui;
}

void Form01::addText(const QString &str) const
{
    ui->textEdit->append(str);
}

QString Form01::getText()
{
    return "헬로 : " +  ui->textEdit->toPlainText();
}

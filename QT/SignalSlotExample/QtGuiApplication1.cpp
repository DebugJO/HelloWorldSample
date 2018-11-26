#include "QtGuiApplication1.h"
#include <QMessageBox>


QtGuiApplication1::QtGuiApplication1(QWidget *parent) : QMainWindow(parent)
{
	ui.setupUi(this);
	connect(ui.pushButton, SIGNAL(clicked()), this, SLOT(SetLabel()));
}

void QtGuiApplication1::SetLabel()
{
	
	ui.label->setText(toKor("헬로우월드"));

	QMessageBox::information(this,toKor("정보"), toKor("내용문구"), toKor("예"));
}

QString QtGuiApplication1::toKor(const char* str)
{
	return QString::fromLocal8Bit(str);
}

#pragma once

#include "ui_QtGuiApplication1.h"

class QtGuiApplication1 : public QMainWindow
{
	Q_OBJECT

public:
	explicit QtGuiApplication1(QWidget *parent = Q_NULLPTR);

private:
	Ui::QtGuiApplication1Class ui{};

public slots:
	void SetLabel();

public:
	static QString toKor(const char* str);
};

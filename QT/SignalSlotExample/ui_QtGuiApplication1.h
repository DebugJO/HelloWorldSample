/********************************************************************************
** Form generated from reading UI file 'QtGuiApplication1.ui'
**
** Created by: Qt User Interface Compiler version 5.11.2
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_QTGUIAPPLICATION1_H
#define UI_QTGUIAPPLICATION1_H

#include <QtCore/QVariant>
#include <QtGui/QIcon>
#include <QtWidgets/QApplication>
#include <QtWidgets/QLabel>
#include <QtWidgets/QMainWindow>
#include <QtWidgets/QPushButton>
#include <QtWidgets/QWidget>

QT_BEGIN_NAMESPACE

class Ui_QtGuiApplication1Class
{
public:
    QWidget *centralWidget;
    QPushButton *pushButton;
    QLabel *label;

    void setupUi(QMainWindow *QtGuiApplication1Class)
    {
        if (QtGuiApplication1Class->objectName().isEmpty())
            QtGuiApplication1Class->setObjectName(QStringLiteral("QtGuiApplication1Class"));
        QtGuiApplication1Class->resize(798, 532);
        QFont font;
        font.setFamily(QString::fromUtf8("\353\247\221\354\235\200 \352\263\240\353\224\225"));
        QtGuiApplication1Class->setFont(font);
        QIcon icon;
        icon.addFile(QStringLiteral("E:/A_C_project20181023/A_Work/ImageLibrary/Windows10/Icon_21.ico"), QSize(), QIcon::Normal, QIcon::Off);
        QtGuiApplication1Class->setWindowIcon(icon);
        centralWidget = new QWidget(QtGuiApplication1Class);
        centralWidget->setObjectName(QStringLiteral("centralWidget"));
        pushButton = new QPushButton(centralWidget);
        pushButton->setObjectName(QStringLiteral("pushButton"));
        pushButton->setGeometry(QRect(140, 110, 131, 61));
        QIcon icon1;
        icon1.addFile(QStringLiteral("E:/A_C_project20181023/A_Work/ImageLibrary/Windows10/Icon_271.ico"), QSize(), QIcon::Normal, QIcon::Off);
        pushButton->setIcon(icon1);
        label = new QLabel(centralWidget);
        label->setObjectName(QStringLiteral("label"));
        label->setGeometry(QRect(140, 190, 171, 16));
        QtGuiApplication1Class->setCentralWidget(centralWidget);

        retranslateUi(QtGuiApplication1Class);

        QMetaObject::connectSlotsByName(QtGuiApplication1Class);
    } // setupUi

    void retranslateUi(QMainWindow *QtGuiApplication1Class)
    {
        QtGuiApplication1Class->setWindowTitle(QApplication::translate("QtGuiApplication1Class", "\354\234\210\353\217\204\354\232\260\354\230\210\354\240\234", nullptr));
        pushButton->setText(QApplication::translate("QtGuiApplication1Class", "\353\210\214\353\237\254\354\243\274\354\202\274", nullptr));
        label->setText(QApplication::translate("QtGuiApplication1Class", "TextLabel", nullptr));
    } // retranslateUi

};

namespace Ui {
    class QtGuiApplication1Class: public Ui_QtGuiApplication1Class {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_QTGUIAPPLICATION1_H

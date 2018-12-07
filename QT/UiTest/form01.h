#ifndef FORM01_H
#define FORM01_H

#include <QWidget>

namespace Ui {
class Form01;
}

class Form01 : public QWidget
{
    Q_OBJECT
    Q_INVOKABLE void adjustSize() { QWidget::adjustSize(); }

public:
    explicit Form01(QWidget *parent = nullptr);
    ~Form01();

public:
    Ui::Form01 *ui;

    void addText(const QString &str) const;
    QString getText();
};

#endif // FORM01_H

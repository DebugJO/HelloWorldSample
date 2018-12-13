#include <QApplication>
#include "ticker.h"

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);

    QPalette p = QApplication::palette();
    p.setColor( QPalette::Text, Qt::red );
    p.setColor( QPalette::WindowText, Qt::red );
    p.setColor( QPalette::ButtonText, Qt::red );
    p.setColor( QPalette::BrightText, Qt::red );
    QApplication::setPalette(p);

    Ticker ticker;
    ticker.setWindowTitle("틱커");
    ticker.setText("How long it lasted was impossible to  say ++ : 한글...");
    ticker.show();

    return a.exec();
}

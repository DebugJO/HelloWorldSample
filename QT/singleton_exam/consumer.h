#ifndef CONSUMER_H
#define CONSUMER_H

#include "test.h"
#include <QDebug>
#include <QObject>

class Consumer : public QObject
{
    Q_OBJECT
  public:
    explicit Consumer(QObject *parent = nullptr);

  signals:

  public slots:
    void TestSlot();
};

#endif // CONSUMER_H

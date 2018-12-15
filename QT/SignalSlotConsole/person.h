#ifndef PERSON_H
#define PERSON_H

#include <QObject>

class Person : public QObject
{
    Q_OBJECT
public:
    explicit Person(QObject *parent = nullptr);
    ~Person();

    void setName(const QString &name)
    {
        m_name = name;
    }

    void speaks(const QString &words);

signals:
    void speak(QString);

public slots:
    void listen(const QString &words);

private:
    QString m_name;
};

#endif // PERSON_H

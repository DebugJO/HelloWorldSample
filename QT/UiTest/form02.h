#ifndef FORM02_H
#define FORM02_H

#include <QWidget>
#include <QAbstractItemModel>
#include <QItemSelectionModel>
#include <QTableView>
#include <QAction>
#include <QModelIndex>
#include <QDebug>
#include <QMessageBox>
#include <QStandardItemModel>
#include <list>

using namespace std;

namespace Ui {
class Form02;
}

class Form02 : public QWidget
{
    Q_OBJECT

public:
    explicit Form02(QWidget *parent = nullptr);
    ~Form02();

public:
    Ui::Form02 *ui;

    void setHeader();
    void tableAddData();
    void createAction();

private slots:
    void slotTabClick(const QModelIndex &index);

private:
    struct searchEntry
    {
        char name[40];
        unsigned int kor;
        unsigned int eng;
        unsigned int math;
    };

    typedef struct searchEntry SEARCH_ENTRY;
    list<SEARCH_ENTRY> listSearchEntry;

    QAbstractItemModel *tabModel;
    QItemSelectionModel *tabSelectionModel;
};

#endif // FORM02_H

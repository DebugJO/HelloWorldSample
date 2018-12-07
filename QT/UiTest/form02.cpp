#include "form02.h"
#include "ui_form02.h"

Form02::Form02(QWidget *parent) : QWidget(parent), ui(new Ui::Form02)
{
    ui->setupUi(this);
    setWindowFlag(Qt::WindowStaysOnTopHint);

    tabModel = new QStandardItemModel(0, 4, this);
    ui->tableView->setModel(tabModel);

    tabSelectionModel = new QItemSelectionModel(tabModel);
    ui->tableView->setSelectionModel(tabSelectionModel);

    ui->tableView->setSelectionBehavior(QAbstractItemView::SelectRows);

    setHeader();
    tableAddData();
    createAction();
}

Form02::~Form02()
{
    delete ui;
}

void Form02::setHeader()
{
    tabModel->setHeaderData(0, Qt::Horizontal, "이름");
    tabModel->setHeaderData(1, Qt::Horizontal, "국어");
    tabModel->setHeaderData(2, Qt::Horizontal, "영어");
    tabModel->setHeaderData(3, Qt::Horizontal, "수학");
}

void Form02::tableAddData()
{
    struct searchEntry dsEntry;

    list<SEARCH_ENTRY>::iterator ds = listSearchEntry.begin();

    memset(&dsEntry, 0x00, sizeof(dsEntry));
    strcpy(dsEntry.name, "홍길동");
    dsEntry.kor = 90;
    dsEntry.eng = 80;
    dsEntry.math = 99;
    advance(ds, 0);
    listSearchEntry.insert(ds, dsEntry);

    tabModel->insertRows(0, 1, QModelIndex());
    tabModel->setData(tabModel->index(0, 0, QModelIndex()), dsEntry.name);
    tabModel->setData(tabModel->index(0, 1, QModelIndex()), dsEntry.kor);
    tabModel->setData(tabModel->index(0, 2, QModelIndex()), dsEntry.eng);
    tabModel->setData(tabModel->index(0, 3, QModelIndex()), dsEntry.math);

    memset(&dsEntry, 0x00, sizeof(dsEntry));
    strcpy(dsEntry.name, "이순신");
    dsEntry.kor = 88;
    dsEntry.eng = 77;
    dsEntry.math = 89;
    //advance(ds, 1);
    listSearchEntry.insert(ds, dsEntry);

    tabModel->insertRows(1, 1, QModelIndex());
    tabModel->setData(tabModel->index(1, 0, QModelIndex()), dsEntry.name);
    tabModel->setData(tabModel->index(1, 1, QModelIndex()), dsEntry.kor);
    tabModel->setData(tabModel->index(1, 2, QModelIndex()), dsEntry.eng);
    tabModel->setData(tabModel->index(1, 3, QModelIndex()), dsEntry.math);
}

void Form02::createAction()
{
    connect(ui->tableView, SIGNAL(clicked(const QModelIndex&)), this, SLOT(slotTabClick(const QModelIndex)));
}

void Form02::slotTabClick(const QModelIndex &index)
{
    struct searchEntry dsEntry;

    qDebug() << "Index Rows : " << index.row();
    qDebug() << "Index Column : " << index.column();

    list<SEARCH_ENTRY>::iterator ds = listSearchEntry.begin();
    advance(ds, index.row());
    dsEntry = *ds;

    if(index.column() == 0)
    {
        QMessageBox::information(this, "선택값", QString("선택한 값 : <font color='Blue'><strong>") + dsEntry.name + QString("</strong></font>"));
    }
    else if (index.column() == 1)
    {
        QMessageBox::information(this, "선택값", QString("선택한 값 : <font color='Blue'><strong>") + QString::number(dsEntry.kor) + QString("</strong></font>"));
    }
    else if (index.column() == 2)
    {
        QMessageBox::information(this, "선택값", QString("선택한 값 : <font color='Blue'><strong>") + QString::number(dsEntry.eng) + QString("</strong></font>"));
    }
    else if (index.column() == 3)
    {
        QMessageBox::information(this, "선택값", QString("선택한 값 : <font color='Blue'><strong>") + QString::number(dsEntry.math) + QString("</strong></font>"));
    }
}

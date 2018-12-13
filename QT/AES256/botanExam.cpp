#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "botan/rng.h"
#include "botan/auto_rng.h"
#include "botan/pipe.h"
#include "botan/aes.h"
#include "botan/filters.h"

MainWindow::MainWindow(QWidget *parent) : QMainWindow(parent), ui(new Ui::MainWindow)
{
    ui->setupUi(this);

    //Botan::InitializationVector init;
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_pushButtonEncrypt_clicked()
{
    try
    {
    // Botan::AutoSeeded_RNG rng;
    // Botan::InitializationVector IV("1234");
    // Botan::SymmetricKey key(rng, 16);
    Botan::SymmetricKey key("1234567890123456789012345678901212345678901234567890123456789012");
    // Botan::Pipe pipe(Botan::get_cipher("AES-256/CBC", key, IV, Botan::ENCRYPTION), new Botan::Hex_Encoder);
    Botan::Pipe pipe(Botan::get_cipher("AES-256/CBC", key, Botan::ENCRYPTION), new Botan::Hex_Encoder);
    pipe.process_msg(ui->lineEditSourceText->text().toStdString());
    std::string m1 = pipe.read_all_as_string(0);
    ui->lineEditEncryptText->setText(QString::fromStdString(m1));
    }
    catch (Botan::Exception ex)
    {
        ui->lineEditEncryptText->setText(ex.what());
    }
}

void MainWindow::on_pushButtonDecrypt_clicked()
{
    try
    {
        Botan::SymmetricKey key("1234567890123456789012345678901212345678901234567890123456789012");
        Botan::Pipe pipe1(new Botan::Hex_Decoder, Botan::get_cipher("AES-256/CBC", key, Botan::DECRYPTION));
        pipe1.process_msg(ui->lineEditEncryptText->text().toStdString());
        std::string m2 = pipe1.read_all_as_string(0);
        ui->lineEditDecryptText->setText(QString::fromStdString(m2));
    }
    catch (Botan::Exception ex)
    {
        ui->lineEditDecryptText->setText(ex.what());
    }
}

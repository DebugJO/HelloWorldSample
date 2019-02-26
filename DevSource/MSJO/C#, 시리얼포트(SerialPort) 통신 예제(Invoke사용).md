### 시리얼 포트 통신 예제 (C#)

```
using System.IO.Ports;

namespace SerialExm
{
    public partial class FrmMain : Form
    {
        private SerialPort SP;

        private void FrmMain_Load(object sender, EventArgs e)
        {
            // CheckForIllegalCrossThreadCalls = false;
            txtComNum.Text = "COM3";
            txtBaudRate.Text = "115200";
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (SP == null)
                {
                    SP = new SerialPort()
                    {
                        PortName = txtComNum.Text,
                        BaudRate = Convert.ToInt32(txtBaudRate.Text),
                        Parity = Parity.None,
                        DataBits = 8,
                        StopBits = StopBits.One,
                        //Handshake = Handshake.RequestToSend,
                        Handshake = Handshake.None,
                        Encoding = Encoding.UTF8, //한글처리 필요시
                        NewLine = "\r",
                        WriteTimeout = 1000
                    };
                    SP.DataReceived += SP_DataReceived;
                    SP.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                SP.Dispose();
                SP = null;
            }
        }

        private void SP_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string receivedData;
            try
            {
                receivedData = SP.ReadExisting();
                receivedData = receivedData.Replace(SP.NewLine, "\r\n");
            }
            catch (Exception ex)
            {
                receivedData = ex.Message;
            }
            AddRecievedDataDelegate add = new AddRecievedDataDelegate(AddRecievedData);
            txtRXData.Invoke(add, receivedData);
        }

        private delegate void AddRecievedDataDelegate(string data);

        private void AddRecievedData(string data)
        {
            txtRXData.Text += data;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SP.Write(txtMessage.Text);
        }

        //private void btnTest_Click(object sender, EventArgs e)
        //{
        //    chart1.Series.Clear();
        //    Series sSin = chart1.Series.Add("sin");
        //    sSin.ChartType = SeriesChartType.Line;
        //    sSin.IsVisibleInLegend = false;
        //    chart1.Series["sin"].Color = Color.Red;
        //    for (double k = 0; k < 2 * Math.PI; k += 0.1)
        //    {
        //        sSin.Points.AddXY(k, Math.Sin(k));
        //    }
        //}
    }
}
```

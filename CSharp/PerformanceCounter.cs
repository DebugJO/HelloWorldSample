using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        readonly PerformanceCounter cpuCounter;
        readonly PerformanceCounter cpuCounter1;
        readonly PerformanceCounter ramCounter;

        public Form1()
        {
            InitializeComponent();

            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter1 = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public string GetCurrentCpuUsage()
        {
            return cpuCounter1.NextValue() + "%";
        }

        public string GetAvailableRAM()
        {
            return ramCounter.NextValue() + "MB";
        }

        private void TimerPFC_Tick(object sender, System.EventArgs e)
        {
            ProgressBarCPU.Invoke(new Action(delegate
            {
                ProgressBarCPU.Value = (int)Math.Ceiling(cpuCounter.NextValue());
                ProgressBarCPU.Refresh();
            }));

            TextBoxCPU.Invoke(new Action(delegate
            {
                TextBoxCPU.Text = GetCurrentCpuUsage();
                TextBoxCPU.Refresh();
            }));

            TextBoxRam.Invoke(new Action(delegate
            {
                TextBoxRam.Text = GetAvailableRAM();
                TextBoxRam.Refresh();
            }));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TimerPFC.Start();
        }
    }
}

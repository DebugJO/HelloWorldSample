using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace YieldReturnAsync
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        // WPF
        // https://www.youtube.com/watch?v=w3hc7nxXxf4
        // https://github.com/brianlagunas/YieldReturnAsync
        private async void ReadFile_Click(object sender, RoutedEventArgs e)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "file.txt");

            var lines = GetLines(path);
            await foreach (var line in lines)
            {
                ListBox.Items.Add(line);
            }
        }

        private static async IAsyncEnumerable<string> GetLines(string filePath)
        {
            string line;
            using StreamReader file = new(filePath);
            while ((line = await file.ReadLineAsync()) != null)
            {
                await Task.Delay(300);
                yield return line;
            }
        }
    }
}

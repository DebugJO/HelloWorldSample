using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncWinfomDemo
{
    public partial class FormMain : Form
    {
        CancellationTokenSource cts;

        public FormMain()
        {
            InitializeComponent();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            if (cts != null)
            {
                cts.Cancel();
            }
        }

        private void ButtonNormal_Click(object sender, System.EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            RunDownloadSync();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            TextBoxResult.AppendLine($"Total execution time: {elapsedMs}");
        }

        private async void ButtonAsync_Click(object sender, EventArgs e)
        {
            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += ReportProgress;

            var watch = System.Diagnostics.Stopwatch.StartNew();

            if (cts != null)
            {
                cts.Dispose();
            }

            cts = new CancellationTokenSource();

            try
            {
                var results = await RunDownloadAsync(progress, cts.Token);
                PrintResults(results);
            }
            catch (OperationCanceledException)
            {
                TextBoxResult.AppendLine($"The async download was canceled.");
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Application.DoEvents();

            TextBoxResult.AppendLine($"Total execution time: {elapsedMs}");
        }

        private async void ButtonParallel_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            await RunDownloadParallelAsync();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            TextBoxResult.AppendLine($"Total execution time: {elapsedMs}");
        }

        private async void ButtonParallel2_Click(object sender, EventArgs e)
        {
            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += ReportProgress;

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var results = await RunDownloadParallelAsync2(progress);
            PrintResults(results);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            TextBoxResult.AppendLine($"Total execution time: {elapsedMs}");
        }

        private void ReportProgress(object sender, ProgressReportModel e)
        {
            ProgressBarDashBoard.Value = e.PercentageComplete;
            PrintResults(e.SiteDownloaded);
        }

        private void RunDownloadSync()
        {
            List<string> websites = PrepData();

            foreach (string site in websites)
            {
                WebSiteDataModel results = DownloadWebsite(site);
                ReportWebsiteInfo(results);
            }
        }

        private async Task<List<WebSiteDataModel>> RunDownloadAsync(IProgress<ProgressReportModel> progress, CancellationToken cancellationToken)
        {
            List<string> websites = PrepData();
            List<WebSiteDataModel> output = new List<WebSiteDataModel>();
            ProgressReportModel report = new ProgressReportModel();

            foreach (string site in websites)
            {
                WebSiteDataModel results = await DownloadWebsiteAsync(site);
                output.Add(results);

                cancellationToken.ThrowIfCancellationRequested();

                report.SiteDownloaded = output;
                report.PercentageComplete = (output.Count * 100) / websites.Count;
                progress.Report(report);
            }

            return output;
        }

        private async Task RunDownloadParallelAsync()
        {
            List<string> websites = PrepData();
            List<Task<WebSiteDataModel>> tasks = new List<Task<WebSiteDataModel>>();

            foreach (string site in websites)
            {
                tasks.Add(DownloadWebsiteAsync(site));
            }

            var result = await Task.WhenAll(tasks);

            foreach (var item in result)
            {
                ReportWebsiteInfo(item);
            }
        }

        private async Task<List<WebSiteDataModel>> RunDownloadParallelAsync2(IProgress<ProgressReportModel> progress)
        {
            List<string> websites = PrepData();
            List<WebSiteDataModel> output = new List<WebSiteDataModel>();
            ProgressReportModel report = new ProgressReportModel();

            await Task.Run(() =>
            {
                Parallel.ForEach<string>(websites, (site) =>
                {
                    WebSiteDataModel results = DownloadWebsite(site);
                    output.Add(results);

                    report.SiteDownloaded = output;
                    report.PercentageComplete = (output.Count * 100) / websites.Count;
                    progress.Report(report);
                });
            });

            return output;
        }


        private static WebSiteDataModel DownloadWebsite(string websiteURL)
        {
            WebSiteDataModel output = new WebSiteDataModel();
            WebClient client = new WebClient();

            output.WebsiteUrl = websiteURL;
            output.WebsiteData = client.DownloadString(websiteURL);

            return output;
        }


        private static async Task<WebSiteDataModel> DownloadWebsiteAsync(string websiteURL)
        {
            WebSiteDataModel output = new WebSiteDataModel();
            WebClient client = new WebClient();

            output.WebsiteUrl = websiteURL;
            output.WebsiteData = await client.DownloadStringTaskAsync(websiteURL);

            return output;
        }

        private List<string> PrepData()
        {
            List<string> output = new List<string>();
            TextBoxResult.Text = "";

            output.Add("https://www.yahoo.com");
            output.Add("https://www.google.com");
            output.Add("https://www.microsoft.com");
            output.Add("https://www.codeproject.com");
            output.Add("https://www.stackoverflow.com");
            output.Add("https://www.cnn.com");

            return output;
        }

        private void ReportWebsiteInfo(WebSiteDataModel data)
        {
            TextBoxResult.AppendLine($"{data.WebsiteUrl} downloaded: {data.WebsiteData.Length} characters long.");
        }

        private void PrintResults(List<WebSiteDataModel> results)
        {
            TextBoxResult.Text = "";

            foreach (var item in results)
            {
                TextBoxResult.AppendLine($"{item.WebsiteUrl} downloaded: {item.WebsiteData.Length} characters long.");
            }
        }
    }

    public class WebSiteDataModel
    {
        public string WebsiteUrl { get; set; } = "";
        public string WebsiteData { get; set; } = "";
    }

    public class ProgressReportModel
    {
        public int PercentageComplete { get; set; } = 0;
        public List<WebSiteDataModel> SiteDownloaded { get; set; } = new List<WebSiteDataModel>();
    }

    public static class MyExtension
    {
        public static void AppendLine(this TextBox source, string value)
        {
            if (source.Text.Length == 0)
                source.Text = value;
            else
                source.AppendText(Environment.NewLine + value);
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncWinform
{
    public partial class FormMain : Form
    {
        private readonly string apiURL;
        private readonly HttpClient httpClient;

        public FormMain()
        {
            InitializeComponent();
            apiURL = "https://localhost:44328/api";
            httpClient = new HttpClient();
        }

        private async void ButtonStart_Click(object sender, System.EventArgs e)
        {
            var progressReport = new Progress<int>(ReportCardProgress);

            TextBoxResult.Clear();
            ProgressBarCards.Value = 0;

            PictureBoxLoding.Visible = true;
            // await Wait();
            // var name = TextBoxInput.Text;

            var stopwatch = new Stopwatch();
            try
            {
                // var greeting = await GetGreetings(name);
                // MessageBox.Show(greeting);

                var cards = await GetCards(1000);
                stopwatch.Start();
                await ProcessCards(cards, progressReport);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show($"Operation done in {stopwatch.ElapsedMilliseconds / 1000.0} seconds");
            PictureBoxLoding.Visible = false;
        }

        private void ReportCardProgress(int percentage)
        {
            ProgressBarCards.Value = percentage;
        }

        private async Task ProcessCards(List<string> cards, IProgress<int> progress = null)
        {
            //int i = 1;

            using var semaphore = new SemaphoreSlim(500);

            var tasks = new List<Task<HttpResponseMessage>>();

            var tasksResolved = 0;

            tasks = cards.Select(async card =>
            {
                var json = JsonConvert.SerializeObject(card);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                await semaphore.WaitAsync();

                try
                {
                    //TextBoxResult.Invoke(new Action(delegate
                    //{
                    //    if (i % 100 == 0)
                    //        TextBoxResult.AppendLine($"SemaphoreSlim : {i}");
                    //    i++;
                    //}));

                    var internalTask = await httpClient.PostAsync($"{apiURL}/cards", content);

                    if (progress != null)
                    {
                        tasksResolved++;
                        var percentage = (double)tasksResolved / cards.Count;
                        percentage *= 100;
                        var percentageInt = (int)Math.Round(percentage, 0);
                        progress.Report(percentageInt);
                    }

                    return internalTask;
                }
                finally
                {
                    semaphore.Release();
                }

            }).ToList();

            var responses = await Task.WhenAll(tasks);
            var rejectedCards = new List<string>();

            foreach (var response in responses)
            {
                var content = await response.Content.ReadAsStringAsync();
                var responseCard = JsonConvert.DeserializeObject<CardResponse>(content);
                if (!responseCard.Approved)
                {
                    rejectedCards.Add(responseCard.Card);
                }
            }

            //int i = 1;
            //ProgressBarCards.Maximum = rejectedCards.Count;
            foreach (var card in rejectedCards)
            {
                TextBoxResult.AppendLine($"Card {card} was rejected");
                Thread.Sleep(1);
                //Application.DoEvents();
                //ProgressBarCards.Value = i;
                //i++;
            }


            //var tasks = new List<Task<HttpResponseMessage>>();
            //await Task.Run(() =>
            //{
            //    foreach (var card in cards)
            //    {
            //        var json = JsonConvert.SerializeObject(card);
            //        var content = new StringContent(json, Encoding.UTF8, "application/json");
            //        var responseTask = httpClient.PostAsync($"{apiURL}/cards", content);
            //        tasks.Add(responseTask);
            //    }
            //});
            //await Task.WhenAll(tasks);
        }

        private static async Task<List<string>> GetCards(int amountOfCardsToGenerate)
        {
            return await Task.Run(() =>
            {
                var cards = new List<string>();
                for (int i = 0; i < amountOfCardsToGenerate; i++)
                {
                    cards.Add(i.ToString().PadLeft(16, '0'));
                }
                return cards;
            });
        }

        private static async Task Wait()
        {
            await Task.Delay(500);
        }

        private async Task<string> GetGreetings(string name)
        {
            using var response = await httpClient.GetAsync($"{apiURL}/greetings/{name}");
            response.EnsureSuccessStatusCode();
            var greeting = await response.Content.ReadAsStringAsync();
            return greeting;
        }
    }

    public class CardResponse
    {
        public string Card { get; set; }
        public bool Approved { get; set; }
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
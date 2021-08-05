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
        private CancellationTokenSource cts;

        public FormMain()
        {
            InitializeComponent();
            apiURL = "https://localhost:5001/api";
            httpClient = new HttpClient();
        }

        private async void ButtonStart_Click(object sender, System.EventArgs e)
        {
            cts = new CancellationTokenSource();

            var progressReport = new Progress<int>(ReportCardProgress);

            TextBoxResult.Clear();

            PictureBoxLoding.Visible = true;

            var stopwatch = new Stopwatch();
            try
            {
                var cards = await GetCards(1000);
                stopwatch.Start();
                await ProcessCards(cards, progressReport, cts.Token);
            }
            catch (TaskCanceledException ex)
            {
                MessageBox.Show("The operation was cancelled : " + ex.Message);
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cts.Dispose();
            }

            MessageBox.Show($"Operation done in {stopwatch.ElapsedMilliseconds / 1000.0} seconds");
            PictureBoxLoding.Visible = false;
            ProgressBarCards.Value = 0;
            cts = null;
        }

        private void ReportCardProgress(int percentage)
        {
            ProgressBarCards.Value = percentage;
        }

        private async Task ProcessCards(List<string> cards, IProgress<int> progress = null, CancellationToken token = default)
        {
            using var semaphore = new SemaphoreSlim(100);

            var tasks = new List<Task<HttpResponseMessage>>();

            tasks = cards.Select(async card =>
            {
                var json = JsonConvert.SerializeObject(card);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                await semaphore.WaitAsync(token);

                try
                {
                    var internalTask = await httpClient.PostAsync($"{apiURL}/cards", content, token);

                    return internalTask;
                }
                finally
                {
                    semaphore.Release();
                }

            }).ToList();

            var responsesTasks = Task.WhenAll(tasks);

            if (progress != null)
            {
                while (await Task.WhenAny(responsesTasks, Task.Delay(TimeSpan.FromSeconds(1), token)) != responsesTasks)
                {
                    if (token.IsCancellationRequested)
                        break;
                    var completedTasks = tasks.Where(x => x.IsCompleted).Count();
                    var percentage = (double)completedTasks / tasks.Count;
                    percentage *= 100;
                    var pecentageInt = (int)Math.Round(percentage, 0);
                    progress.Report(pecentageInt);
                }
            }

            var responses = await responsesTasks;

            var rejectedCards = new List<string>();

            foreach (var response in responses)
            {
                var content = await response.Content.ReadAsStringAsync(token);
                var responseCard = JsonConvert.DeserializeObject<CardResponse>(content);
                if (!responseCard.Approved)
                {
                    rejectedCards.Add(responseCard.Card);
                }
            }

            foreach (var card in rejectedCards)
            {
                TextBoxResult.AppendText("Card " + card + " was rejected" + Environment.NewLine);
            }
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

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
            
        }
    }

    public class CardResponse
    {
        public string Card { get; set; }
        public bool Approved { get; set; }
    }
}

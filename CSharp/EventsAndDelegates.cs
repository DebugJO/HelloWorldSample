/* Reference
Programming with Mosh, https://www.youtube.com/watch?v=jQgwEsJISy0
*/

using System;
using System.Threading;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Start...");

            var video = new Video() { Title = "Video 1" };
            var videoEncoder = new VideoEncoder(); // publisher
            var mailService = new MailService(); // subscriber
            var messageService = new MessageService();

            videoEncoder.VideoEncoded += mailService.OnVideoEncoded;
            videoEncoder.VideoEncoded += messageService.OnVideoEncoded;

            videoEncoder.Encode(video);
            Console.WriteLine("End...");
        }
    }

    public class MailService
    {
        public void OnVideoEncoded(object sender, VideoEventArgs e)
        {
            Console.WriteLine("MailService: Sending an email... : " + e.Video.Title);
        }
    }

    public class MessageService
    {
        public void OnVideoEncoded(object sender, VideoEventArgs e)
        {
            Console.WriteLine("MessageService: Sending a text message... : " + e.Video.Title);
        }
    }

    public class Video
    {
        public string Title { get; set; }
    }

    public class VideoEventArgs : EventArgs
    {
        public Video Video { get; set; }
    }

    public class VideoEncoder
    {
        // [1]... delegate 키워드 사용
        //public delegate void VideoEncodeEnventHandler(object sender, VideoEventArgs args);
        //public event VideoEncodeEnventHandler VideoEncoded;

        // [2]... delegate 키워드 미사용
        public event EventHandler<VideoEventArgs> VideoEncoded;

        public void Encode(Video video)
        {
            Console.WriteLine("Encoding Video... : " + video.Title);
            Thread.Sleep(3000);

            OnVideoEncoded(video);
        }

        protected virtual void OnVideoEncoded(Video video)
        {
            //if (VideoEncoded != null)
            //    VideoEncoded(this, EventArgs.Empty);
            VideoEncoded?.Invoke(this, new VideoEventArgs() { Video = video });
        }
    }
}

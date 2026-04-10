using System;
using System.Threading;
using ConsoleApp1YTRecap.Events;

namespace ConsoleApp1YTRecap.Videos
{
    public class VideoEncoder
    {
        public event EventHandler<Videos.VideoEventArgs> VideoEncoded;

        public void Encode(Video video)
        {
            Console.WriteLine($"Encoding video: {video.Title}");
            Thread.Sleep(2000); // Simulate time-consuming encoding
            Console.WriteLine("Encoding complete.\n");
            OnVideoEncoded(this, new VideoEventArgs { Video = video });
        }

        // ✓ Correct signature: matches EventHandler<VideoEventArgs>
        protected virtual void OnVideoEncoded(object sender, VideoEventArgs e)
        {
            VideoEncoded?.Invoke(sender, e);
        }

        public static void Run()
        {
            var encoder = new VideoEncoder();
            var mailService = new Events.MailService();
            var messageService = new Events.MessageService();

            encoder.VideoEncoded += mailService.OnVideoEncoded;
            encoder.VideoEncoded += messageService.OnVideoEncoded;

            var video = new Video { Title = "My First C# Tutorial" };
            encoder.Encode(video);
        }
    }
}
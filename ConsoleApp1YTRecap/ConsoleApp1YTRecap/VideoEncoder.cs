using System;
using System.Threading;

namespace ConsoleApp1YTRecap
{
    public class VideoEncoder
    {
        // --- BEFORE (your current code, kept as a comment for learning) ---
        // public delegate void VideoEncodedEventHandler(object sender, EventArgs e);
        // public event VideoEncodedEventHandler VideoEncoded;

        // --- AFTER: Use the built-in EventHandler<T> — no custom delegate needed ---
        // EventHandler<VideoEventArgs> replaces both the delegate declaration AND the event
        public event EventHandler<VideoEventArgs> VideoEncoded;

        // Encode now takes a Video object instead of a plain string
        public void Encode(Video video)
        {
            Console.WriteLine($"Encoding video: {video.Title}");
            Thread.Sleep(2000); // Simulate time-consuming encoding
            Console.WriteLine("Encoding complete.\n");

            OnVideoEncoded(video); // Fire the event
        }

        // Protected virtual: lets subclasses override the raising logic
        // Passes the Video inside VideoEventArgs so subscribers know WHICH video finished
        protected virtual void OnVideoEncoded(Video video)
        {
            // Null-conditional operator: thread-safe, no NullReferenceException if no subscribers
            VideoEncoded?.Invoke(this, new VideoEventArgs { Video = video });
        }

        // --- Static Run() wires everything together (your routing pattern) ---
        public static void Run()
        {
            // 1. Create the publisher
            var encoder = new VideoEncoder();

            // 2. Create subscribers
            var mailService = new MailService();
            var messageService = new MessageService();

            // 3. Subscribe — multiple handlers on the same event (multicast delegate)
            encoder.VideoEncoded += mailService.OnVideoEncoded;
            encoder.VideoEncoded += messageService.OnVideoEncoded;

            // 4. Create a video and encode it
            var video = new Video { Title = "My First C# Tutorial" };
            encoder.Encode(video);
        }
    }
}
using System;

namespace ConsoleApp1YTRecap
{
    // Subscriber: reacts to VideoEncoded event by sending an email
    public class MailService
    {
        // The signature MUST match EventHandler<VideoEventArgs>:
        //   object sender  →  who raised the event (the VideoEncoder)
        //   VideoEventArgs e  →  the extra data (which Video finished)
        public void OnVideoEncoded(object sender, VideoEventArgs e)
        {
            Console.WriteLine($"MailService: Sending email for '{e.Video.Title}'...");
        }
    }
}
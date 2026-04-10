using System;
using System.Text;
using ConsoleApp1YTRecap.Videos;

namespace ConsoleApp1YTRecap.Events
{
    // Subscriber: reacts to the same event — demonstrates multicast delegates
    public class MessageService
    {
        public void OnVideoEncoded(object sender, VideoEventArgs e)
        {
            Console.WriteLine($"MessageService: Sending SMS for '{e.Video.Title}'...");
        }
    }
}

using System;
using System.Text;

namespace ConsoleApp1YTRecap
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

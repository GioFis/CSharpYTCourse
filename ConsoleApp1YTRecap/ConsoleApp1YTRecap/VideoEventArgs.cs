using System;

namespace ConsoleApp1YTRecap
{
    // Custom EventArgs to pass extra data (the Video) when the event fires
    public class VideoEventArgs : EventArgs
    {
        public Video Video { get; set; }
    }
}
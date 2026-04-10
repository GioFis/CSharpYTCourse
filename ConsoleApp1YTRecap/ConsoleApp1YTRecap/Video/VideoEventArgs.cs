using System;

namespace ConsoleApp1YTRecap.Videos
{
    // Custom EventArgs to pass extra data (the Video) when the event fires
    public class VideoEventArgs : EventArgs
    {
        public Videos.Video Video { get; set; }
    }
}
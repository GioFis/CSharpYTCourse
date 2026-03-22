using System;


namespace ConsoleApp1YTRecap
{
    class TestingEventsSubscriber
    {
        //Private copies counter and timestamps
        //They belong to the subscriber now, not the publisher
        //The publisher just fire the bell
        private int _spaceCount = 0;
        private List<DateTime> _pressTimes = new List<DateTime>();

        //Constrcture receive the publisher and immediately subscribes
        //publisher is just a local name for the TestingEvent instance.
        public TestingEventsSubscriber(TestingEvents publisher)
        {
            //Wire up : when publisher fires OnSpacePressed call our handler
            publisher.OnSpacePressed += OnSpacePressed;
        }

        // The handler — signature must still match EventHandler: (object, EventArgs) → void
        private void OnSpacePressed(object sender, EventArgs e)
        {
            _spaceCount++;
            _pressTimes.Add(DateTime.Now);
            Console.WriteLine($"  [Subscriber] Space pressed! Count: {_spaceCount} — at {DateTime.Now:HH:mm:ss.fff}");
        }

        // Called by Run() after the loop ends to print the summary
        public void PrintSummary()
        {
            Console.WriteLine($"  You pressed SPACE {_spaceCount} time(s) in total.");
            Console.WriteLine("  Timestamps:");
            foreach (DateTime t in _pressTimes)
                Console.WriteLine($"    → {t:HH:mm:ss.fff}");
        }
     }
}

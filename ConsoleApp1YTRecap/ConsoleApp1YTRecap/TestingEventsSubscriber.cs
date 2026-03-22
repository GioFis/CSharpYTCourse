using System;
using System.Collections.Generic; // ← ADD THIS (List<T> lives here)
using System.Xml;
namespace ConsoleApp1YTRecap
{
    class TestingEventsSubscriber
    {
        private int _spaceCount = 0;
        private List<DateTime> _pressTimes = new List<DateTime>();
        private List<float> _intensities = new List<float>(); // ← was: NewLineHandling List<float>()

        public TestingEventsSubscriber(TestingEvents publisher)
        {
            publisher.OnSpacePressed += OnSpacePressed;
            publisher.OnSpacePressed += OnSpacePressedLogger;
        }

        // ← was: (object, sender, ...) — the comma made 'sender' a second type parameter
        private void OnSpacePressed(object sender, SpacePressedEventArgs e)
        {
            _spaceCount++;
            _pressTimes.Add(DateTime.Now);
            _intensities.Add(e.Intensity);
            Console.WriteLine($"  [Subscriber] Space pressed! Count: {_spaceCount} | intensity: {e.Intensity:F3} | at {DateTime.Now:HH:mm:ss.fff}");
        } // ← ADD closing brace for OnSpacePressed — it was missing

        // ← was: nested INSIDE OnSpacePressed (private method inside a method is illegal in C#)
        private void OnSpacePressedLogger(object sender, SpacePressedEventArgs e)
        {
            Console.WriteLine($"  [Logger]     Intensity {e.Intensity:F3} logged.");
        }

        public void PrintSummary()
        {
            Console.WriteLine($"  You pressed SPACE {_spaceCount} time(s) in total.");
            Console.WriteLine("  Timestamps:");
            for (int i = 0; i < _pressTimes.Count; i++) // ← was: foreach (no 'i' available in foreach)
                Console.WriteLine($"    → {_pressTimes[i]:HH:mm:ss.fff} , intensity: {_intensities[i]:F3}");
        }
    }
}
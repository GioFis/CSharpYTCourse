// ============================================================
//  TestingEvents.cs  —  EP.4: Events (Console version)
//
//  IMPORTANT: The Brackeys video uses Unity (MonoBehaviour, 
//  Debug.Log, Input.GetKeyDown). This is a CONSOLE APP, so:
//
//    MonoBehaviour        →  removed (plain class instead)
//    Debug.Log()          →  Console.WriteLine()
//    Input.GetKeyDown()   →  Console.ReadKey()
//    Start()              →  code at top of Run()
//    Update() game loop   →  while loop
//    using UnityEngine    →  removed (doesn't exist here)
// ============================================================

namespace ConsoleApp1YTRecap
{
    class TestingEvents
    {
        // ── THE EVENT ────────────────────────────────────────────────────
        // EventHandler is a built-in delegate: (object sender, EventArgs e) → void
        // The 'event' keyword means: only THIS class can fire it,
        // but anyone outside can subscribe/unsubscribe with += / -=
        //public event EventHandler OnSpacePressed;
        // EventHandler<T> is the modern built-in generic delegate.
        // T = your custom EventArgs type. Subscriber MUST match this signature.
        public event EventHandler<SpacePressedEventArgs> OnSpacePressed;

        // Random number generator to simulate a float value per press
        private Random _rng = new Random();

        // Tracks how many times spacebar was pressed.
        // Private: only this class can read/modify it.
        private int _spaceCount = 0;
        // Stores the timestamp of each spacebar press.
        // List<DateTime> = a dynamic list that grows every time space is pressed.
        private List<DateTime> _pressTimes = new List<DateTime>();



        // ── EQUIVALENT OF Unity's Start() ────────────────────────────────
        // In Unity, Start() runs once when the object is created.
        // Here we just call this setup logic at the beginning of Run().
        private void Setup()
        {
            // Subscribe: when OnSpacePressed fires, call Testing_OnSpacePressed
            // += means "add this method to the list of listeners"
            //Now in the subscriber TesingEventsSubscriber.cs OnSpacePressed += Testing_OnSpacePressed;
        }

        // ── EVENT HANDLER ─────────────────────────────────────────────────
        // This method's signature MUST match EventHandler:
        //   (object sender, EventArgs e) → void
        // 'sender' = the object that fired the event (the TestingEvents instance)
        // 'e'      = any extra data sent with the event (empty here)
        //private void Testing_OnSpacePressed(object sender, EventArgs e)
        //{
        //    _spaceCount++;  // increment the counter every time this handler is called
        //    _pressTimes.Add(DateTime.Now);  // capture exact time of this press

        //    Console.WriteLine($"  [Event fired] Space pressed! Total count: {_spaceCount}");
        //    Console.WriteLine($"  [Event fired] Space pressed! Count: {_spaceCount} — at {DateTime.Now:HH:mm:ss.fff}");
        //}

        // ── EQUIVALENT OF Unity's Update() ───────────────────────────────
        // In Unity, Update() runs every single frame automatically.
        // In a console app, we simulate this with a while loop.
        private void UpdateLoop()
        {
            Console.WriteLine("  Press SPACEBAR to fire the event. Press ESC to quit.\n");

            while (true)   // runs forever — same idea as Unity's Update()
            {
                // Console.ReadKey(true) reads ONE key press
                // 'true' = don't print the key character to the screen
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Spacebar)
                {
                    // Fire the event — equivalent of Unity's OnSpacePressed?.Invoke(this, EventArgs.Empty)
                    // ?. means: only call Invoke if at least one handler is subscribed
                    // Without ?. this would crash with NullReferenceException if nobody subscribed
                    // Generate a random float between 0.0 and 1.0
                    float intensity = (float)_rng.NextDouble();
                    // Pack the float into the custom EventArgs and fire
                    OnSpacePressed?.Invoke(this, new SpacePressedEventArgs(intensity));
        
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine($"  [Escape pressed — exiting loop]");
                    break;
                }
            }
        }

        // ── PUBLIC ENTRY POINT ────────────────────────────────────────────
        // Called by Program.cs — replaces the need for a static Main here
        public static void Run()
        {
            Console.WriteLine("--- EP.4: Testing Events ---\n");

            // We need an INSTANCE because OnSpacePressed is not static —
            // it belongs to each individual object, not the class itself.
            // (Unity does this for you behind the scenes with MonoBehaviour)
            TestingEvents demo = new TestingEvents();
            TestingEventsSubscriber subscriber = new TestingEventsSubscriber(demo);  // create a subscriber and pass the publisher to its constructor
            //demo.Setup();       // subscribe handlers — like Unity's Start()
            demo.UpdateLoop();  // run the input loop — like Unity's Update()
            subscriber.PrintSummary(); // print the summary of spacebar presses after exiting the loop, now lives in the subscriber
        }
    }
}
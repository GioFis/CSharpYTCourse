// ============================================================
//  EventsDemo.cs  —  EP.3: Delegates & Events
//
//  THE MENTAL MODEL (think of it like AWS SNS):
//
//    SNS Topic      →  event
//    SNS Message    →  EventArgs (the data sent with the event)
//    Publisher      →  the class that calls .Invoke()
//    Subscriber     →  any method registered with +=
//    Lambda/SQS     →  the handler method that reacts
//
//  3-step recipe every time:
//    1. Declare a delegate (defines the METHOD SHAPE handlers must match)
//    2. Declare an event of that delegate type  (the "bell")
//    3. Subscribe with +=, unsubscribe with -=
// ============================================================

using System;

namespace ConsoleApp1YTRecap
{
    // ── STEP 1: DECLARE THE DELEGATE ─────────────────────────────────────────
    // A delegate is a TYPE that describes what a method must look like.
    // This one says: "any method that takes (object, EventArgs) and returns void"
    // is a valid handler for this delegate.
    //
    // Python equivalent: a type hint like Callable[[object, EventArgs], None]
    public delegate void PlayerEventHandler(object sender, EventArgs args);


    // ── PUBLISHER CLASS ───────────────────────────────────────────────────────
    // This class OWNS the event and decides WHEN to fire it.
    // Think of it as the SNS topic definition + the thing that publishes to it.
    class Player
    {
        public string Name { get; set; }
        private int _health = 100;

        // ── STEP 2: DECLARE THE EVENT ────────────────────────────────────────
        // The event keyword wraps the delegate — it restricts outsiders:
        //   - outsiders CAN subscribe/unsubscribe  (+=  -=)
        //   - outsiders CANNOT fire it directly    (only this class can Invoke)
        public event PlayerEventHandler OnDamaged;   // fires whenever player takes damage
        public event PlayerEventHandler OnDied;      // fires when health reaches 0

        public Player(string name) { Name = name; }

        // Public method that triggers the event internally
        public void TakeDamage(int amount)
        {
            _health -= amount;
            Console.WriteLine($"  {Name} took {amount} damage. HP: {_health}");

            // Raise OnDamaged — the ?. means "only call if someone subscribed"
            // Without ?. this would crash with NullReferenceException if nobody subscribed
            OnDamaged?.Invoke(this, EventArgs.Empty);

            if (_health <= 0)
            {
                _health = 0;
                OnDied?.Invoke(this, EventArgs.Empty);  // raise death event
            }
        }

        public int GetHealth() { return _health; }
    }


    // ── SUBSCRIBER CLASS ──────────────────────────────────────────────────────
    // This class LISTENS to events from Player.
    // It doesn't need to know how Player works internally — loose coupling.
    // In AWS terms: this is your Lambda that's subscribed to the SNS topic.
    class HUD
    {
        // This method's signature must MATCH the delegate:
        // (object sender, EventArgs args) → void
        public void OnPlayerDamaged(object sender, EventArgs args)
        {
            Player p = (Player)sender;   // cast sender back to Player to access its members
            Console.WriteLine($"  [HUD] Updating health bar for {p.Name}. HP left: {p.GetHealth()}");
        }

        public void OnPlayerDied(object sender, EventArgs args)
        {
            Player p = (Player)sender;
            Console.WriteLine($"  [HUD] GAME OVER screen — {p.Name} has died.");
        }
    }


    // ── DEMO RUNNER ───────────────────────────────────────────────────────────
    class EventsDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- EP.3: Events & Delegates ---\n");

            Player player = new Player("Arthur");
            HUD hud = new HUD();

            // ── STEP 3: SUBSCRIBE ─────────────────────────────────────────
            // += means "add this method to the list of listeners"
            // You can add as many handlers as you want — all will be called
            player.OnDamaged += hud.OnPlayerDamaged;   // HUD listens to damage
            player.OnDied += hud.OnPlayerDied;      // HUD listens to death

            // You can also subscribe a second independent handler to the same event
            player.OnDied += LogToConsole;             // a standalone logger also listens

            // Now fire some events by calling the public method
            Console.WriteLine("[Scenario] Arthur enters battle...\n");
            player.TakeDamage(30);
            Console.WriteLine();
            player.TakeDamage(40);
            Console.WriteLine();
            player.TakeDamage(50);  // this will drop HP to 0 and fire OnDied
            Console.WriteLine();

            // ── UNSUBSCRIBE ───────────────────────────────────────────────
            // -= removes a handler — useful for cleanup or conditional listening
            player.OnDamaged -= hud.OnPlayerDamaged;
            Console.WriteLine("[HUD unsubscribed from OnDamaged]");
            Console.WriteLine("[Any further damage won't update the HUD]\n");


            // ── EVENTHANDLER<T>: THE MODERN RECOMMENDED PATTERN ──────────
            // Instead of defining a custom delegate, .NET provides built-in:
            //   EventHandler           — for events with no extra data
            //   EventHandler<TArgs>    — for events WITH custom data
            // This is what you'll see in almost all real .NET code.
            Console.WriteLine("--- Modern pattern with EventHandler<T> ---\n");

            Door door = new Door();
            door.OnOpened += (sender, args) =>
            {
                // Lambda as event handler — equivalent to defining a separate method
                // Python: door.on_opened = lambda sender, args: print(...)
                DoorEventArgs e = (DoorEventArgs)args;
                Console.WriteLine($"  [Listener] Door was opened by: {e.OpenedBy}");
            };

            door.Open("Giovanni");
            door.Open("Brackeys");
        }

        // A standalone static method also qualifies as a handler
        // as long as its signature matches the delegate
        static void LogToConsole(object sender, EventArgs args)
        {
            Console.WriteLine($"  [Logger] Event fired. Logging to console.");
        }
    }


    // ── CUSTOM EVENTARGS — SENDING DATA WITH YOUR EVENT ───────────────────────
    // EventArgs is the base class for event data.
    // You subclass it to attach your own payload, like a message body in SNS.
    class DoorEventArgs : EventArgs
    {
        public string OpenedBy { get; set; }           // custom data field
        public DoorEventArgs(string openedBy) { OpenedBy = openedBy; }
    }

    class Door
    {
        // EventHandler<DoorEventArgs> is a built-in generic delegate
        // meaning: handlers must be (object sender, DoorEventArgs args) → void
        public event EventHandler<DoorEventArgs> OnOpened;

        public void Open(string person)
        {
            Console.WriteLine($"  Door opened by {person}.");
            // Pass custom data by creating a DoorEventArgs instance
            OnOpened?.Invoke(this, new DoorEventArgs(person));
        }
    }
}
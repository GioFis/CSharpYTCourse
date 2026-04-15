// ============================================================
//  SingletonDemo.cs  —  EP.10  Singleton Pattern
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  The Singleton is a CREATIONAL design pattern that ensures
//  a class has EXACTLY ONE instance throughout the application,
//  and provides a GLOBAL access point to that instance.
//
//  When to use:
//    • A single shared resource: logger, config, audio manager,
//      game state, database connection pool, event bus
//    • You need one coordinator, not many copies
//
//  When NOT to use:
//    • When multiple independent instances are fine
//    • It makes unit testing harder (hidden global state)
//    • Overused → becomes an anti-pattern ("global variable dressed up")
//
//  Implementations shown:
//    1. Basic (not thread-safe)    — understand the concept
//    2. Thread-safe lazy           — with lock
//    3. Lazy<T>                    — cleanest modern C# version
//    4. Static initialiser         — simplest, eager
//
//  Each implementation enforces the three Singleton rules:
//    a) Private constructor        — nobody can do `new Singleton()`
//    b) Private static instance    — stored inside the class itself
//    c) Public static accessor     — the one door in: Instance or GetInstance()
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    // =========================================================
    //  1. Basic Singleton — simple, NOT thread-safe
    //     Good for understanding; use Lazy<T> in production.
    // =========================================================
    public class BasicSingleton
    {
        // b) Private static field holds THE instance
        private static BasicSingleton? _instance;

        public int AccessCount { get; private set; }
        public string SessionId { get; }

        // a) Private constructor — cannot be called from outside
        private BasicSingleton()
        {
            SessionId = Guid.NewGuid().ToString()[..8];
            Console.WriteLine($"  [BasicSingleton created] id={SessionId}");
        }

        // c) Public accessor — creates on first call, returns same after
        public static BasicSingleton Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BasicSingleton();  // created ONCE
                return _instance;
            }
        }

        public void DoWork(string caller)
        {
            AccessCount++;
            Console.WriteLine($"  BasicSingleton.DoWork() by '{caller}' — access #{AccessCount}");
        }
    }

    // =========================================================
    //  2. Thread-safe Singleton — with double-checked locking
    //     Safe when multiple threads might call Instance at once.
    // =========================================================
    public class ThreadSafeSingleton
    {
        private static ThreadSafeSingleton? _instance;
        private static readonly object _lock = new();

        public int CallCount { get; private set; }

        private ThreadSafeSingleton()
            => Console.WriteLine("  [ThreadSafeSingleton created]");

        public static ThreadSafeSingleton Instance
        {
            get
            {
                if (_instance == null)                // first check (no lock — fast path)
                {
                    lock (_lock)                      // lock only when _instance might be null
                    {
                        if (_instance == null)        // second check (inside lock — safe)
                            _instance = new ThreadSafeSingleton();
                    }
                }
                return _instance;
            }
        }

        public void Log(string message)
        {
            CallCount++;
            Console.WriteLine($"  [LOG #{CallCount}] {message}");
        }
    }

    // =========================================================
    //  3. Lazy<T> Singleton — cleanest modern C# approach
    //     Thread-safe by default. Lazy = created on first access.
    // =========================================================
    public class LazySingleton
    {
        // Lazy<T> handles the null check and thread-safety automatically
        private static readonly Lazy<LazySingleton> _lazy =
            new(() => new LazySingleton());

        public static LazySingleton Instance => _lazy.Value;

        public List<string> EventLog { get; } = new();

        private LazySingleton()
            => Console.WriteLine("  [LazySingleton created] — Lazy<T> handled thread-safety");

        public void RecordEvent(string ev)
        {
            EventLog.Add($"[{DateTime.Now:HH:mm:ss}] {ev}");
            Console.WriteLine($"  Event recorded: {ev}");
        }
    }

    // =========================================================
    //  4. Static initialiser Singleton — simplest of all
    //     Created when the class is first loaded. Always thread-safe.
    //     Downside: created even if never used (eager).
    // =========================================================
    public class EagerSingleton
    {
        // Initialised at class load time — guaranteed single instance
        public static readonly EagerSingleton Instance = new();

        public int Score { get; set; }

        private EagerSingleton()
            => Console.WriteLine("  [EagerSingleton created] — static field initialiser");

        public void AddScore(int points)
        {
            Score += points;
            Console.WriteLine($"  Score += {points} → total {Score}");
        }
    }

    // =========================================================
    //  Real-world example: GameManager Singleton
    // =========================================================
    public class GameManager
    {
        private static readonly Lazy<GameManager> _lazy = new(() => new GameManager());
        public static GameManager Instance => _lazy.Value;

        public int Level { get; private set; } = 1;
        public int Score { get; private set; }
        public bool IsPaused { get; private set; }
        public string PlayerName { get; set; } = "Hero";

        private GameManager() { }

        public void AddScore(int points)
        {
            Score += points;
            Console.WriteLine($"  {PlayerName} scored {points} pts → total {Score}");
        }

        public void NextLevel()
        {
            Level++;
            Console.WriteLine($"  ★ Level up! Now at level {Level}");
        }

        public void Pause() { IsPaused = true; Console.WriteLine("  Game paused."); }
        public void Resume() { IsPaused = false; Console.WriteLine("  Game resumed."); }

        public override string ToString() =>
            $"GameManager | Player={PlayerName}  Level={Level}  Score={Score}  Paused={IsPaused}";
    }

    // =========================================================
    //  Demo runner
    // =========================================================
    public static class SingletonDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Singleton Pattern ===\n");

            BasicSingletonDemo();
            ThreadSafeDemo();
            LazyDemo();
            EagerDemo();
            GameManagerDemo();
            SameInstanceProof();
        }

        private static void BasicSingletonDemo()
        {
            Console.WriteLine("--- 1. Basic Singleton ---");
            Console.WriteLine("  Private constructor + static Instance property.\n");

            // Both variables point to THE SAME object
            var a = BasicSingleton.Instance;
            var b = BasicSingleton.Instance;   // constructor NOT called again

            a.DoWork("ModuleA");
            b.DoWork("ModuleB");

            Console.WriteLine($"  Same instance? {ReferenceEquals(a, b)}");
            Console.WriteLine($"  Total accesses: {a.AccessCount}");   // b.AccessCount == a.AccessCount
        }

        private static void ThreadSafeDemo()
        {
            Console.WriteLine("\n--- 2. Thread-safe Singleton (double-checked lock) ---\n");

            var logger = ThreadSafeSingleton.Instance;
            logger.Log("Application started");
            logger.Log("User logged in");
            ThreadSafeSingleton.Instance.Log("Shared across calls");
            Console.WriteLine($"  Total log calls: {ThreadSafeSingleton.Instance.CallCount}");
        }

        private static void LazyDemo()
        {
            Console.WriteLine("\n--- 3. Lazy<T> Singleton (preferred modern approach) ---\n");

            LazySingleton.Instance.RecordEvent("Game started");
            LazySingleton.Instance.RecordEvent("Player joined");
            LazySingleton.Instance.RecordEvent("Level loaded");

            Console.WriteLine($"\n  Event log ({LazySingleton.Instance.EventLog.Count} entries):");
            foreach (string e in LazySingleton.Instance.EventLog)
                Console.WriteLine($"    {e}");
        }

        private static void EagerDemo()
        {
            Console.WriteLine("\n--- 4. Eager Singleton (static field initialiser) ---\n");

            EagerSingleton.Instance.AddScore(100);
            EagerSingleton.Instance.AddScore(250);
            Console.WriteLine($"  Final score: {EagerSingleton.Instance.Score}");
        }

        private static void GameManagerDemo()
        {
            Console.WriteLine("\n--- 5. Real-world: GameManager Singleton ---\n");

            GameManager.Instance.PlayerName = "Alice";
            GameManager.Instance.AddScore(300);
            GameManager.Instance.NextLevel();
            GameManager.Instance.AddScore(500);
            GameManager.Instance.Pause();
            GameManager.Instance.Resume();
            GameManager.Instance.NextLevel();

            Console.WriteLine($"\n  State: {GameManager.Instance}");
        }

        private static void SameInstanceProof()
        {
            Console.WriteLine("\n--- 6. Proof: all references point to same object ---\n");

            // Simulate three "different parts of the app" accessing GameManager
            var ref1 = GameManager.Instance;   // "UI layer"
            var ref2 = GameManager.Instance;   // "Audio system"
            var ref3 = GameManager.Instance;   // "Save system"

            Console.WriteLine($"  ref1 == ref2: {ReferenceEquals(ref1, ref2)}");
            Console.WriteLine($"  ref2 == ref3: {ReferenceEquals(ref2, ref3)}");
            Console.WriteLine($"  ref1 == ref3: {ReferenceEquals(ref1, ref3)}");
            Console.WriteLine("  → All three are the SAME object in memory.");

            Console.WriteLine("\n  Comparison of implementations:");
            Console.WriteLine("  Basic           — simple, NOT thread-safe");
            Console.WriteLine("  Double-check    — thread-safe, slightly verbose");
            Console.WriteLine("  Lazy<T>         — thread-safe, clean, lazy (recommended)");
            Console.WriteLine("  Static field    — thread-safe, eager (simple but always created)");
        }
    }
}
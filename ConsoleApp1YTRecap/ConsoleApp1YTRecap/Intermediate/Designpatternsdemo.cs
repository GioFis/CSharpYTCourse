// ============================================================
//  DesignPatternsDemo.cs  —  EP.10  Design Patterns
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  Design patterns are PROVEN, NAMED solutions to recurring
//  problems in software design. They are NOT code libraries —
//  they are blueprints you adapt to your situation.
//
//  Three categories:
//    Creational  — how objects are created
//    Structural  — how objects are composed
//    Behavioural — how objects communicate
//
//  Patterns covered here (beyond Singleton from ep.28):
//    1. Observer   (Behavioural) — notify many on state change
//    2. Strategy   (Behavioural) — swap algorithms at runtime
//    3. Factory    (Creational)  — decouple creation from usage
//    4. Command    (Behavioural) — encapsulate an action as an object
//
//  Note: C# events ARE an implementation of the Observer pattern.
//  Here we build it manually so the pattern is visible, then
//  show how events replace the boilerplate.
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    // =========================================================
    //  1. OBSERVER PATTERN
    //  Subject notifies all registered observers when it changes.
    //  Real uses: UI data-binding, event systems, pub/sub buses.
    //
    //  Participants:
    //    IObserver<T>   — the listener contract
    //    IObservable<T> — the source contract  (built into .NET)
    //    Subject        — holds state, notifies observers
    // =========================================================
    public interface IGameObserver
    {
        void OnScoreChanged(int newScore);
        void OnLevelChanged(int newLevel);
    }

    public class ObservableGame
    {
        private readonly List<IGameObserver> _observers = new();
        private int _score;
        private int _level = 1;

        // Subscribe / Unsubscribe
        public void Subscribe(IGameObserver observer) => _observers.Add(observer);
        public void Unsubscribe(IGameObserver observer) => _observers.Remove(observer);

        public void AddScore(int points)
        {
            _score += points;
            NotifyScoreChanged(_score);
            if (_score >= _level * 500) LevelUp();
        }

        private void LevelUp()
        {
            _level++;
            NotifyLevelChanged(_level);
        }

        private void NotifyScoreChanged(int score)
        {
            foreach (var o in _observers) o.OnScoreChanged(score);
        }

        private void NotifyLevelChanged(int level)
        {
            foreach (var o in _observers) o.OnLevelChanged(level);
        }
    }

    // Three different observers react differently to the same events
    public class HudDisplay : IGameObserver
    {
        public void OnScoreChanged(int s) => Console.WriteLine($"  [HUD]   Score: {s}");
        public void OnLevelChanged(int l) => Console.WriteLine($"  [HUD]   ★ Level {l}!");
    }

    public class AchievementSystem : IGameObserver
    {
        public void OnScoreChanged(int s)
        {
            if (s >= 1000) Console.WriteLine("  [ACH]   🏆 Achievement: 1000 pts!");
        }
        public void OnLevelChanged(int l)
        {
            if (l == 2) Console.WriteLine("  [ACH]   🏆 Achievement: Reached Level 2!");
        }
    }

    public class AudioManager : IGameObserver
    {
        public void OnScoreChanged(int s) { /* only cares about level */ }
        public void OnLevelChanged(int l) =>
            Console.WriteLine($"  [AUDIO] ♫ Playing level-{l} theme music");
    }

    // =========================================================
    //  2. STRATEGY PATTERN
    //  Define a FAMILY of algorithms, encapsulate each one,
    //  and make them interchangeable at runtime.
    //  Real uses: sorting, payment, pathfinding, compression.
    //
    //  Participants:
    //    IStrategy    — the algorithm contract
    //    ConcreteX    — specific implementations
    //    Context      — uses an IStrategy, doesn't know which
    // =========================================================
    public interface IDamageStrategy
    {
        int Calculate(int basePower, int targetDefense);
        string Name { get; }
    }

    public class PhysicalDamage : IDamageStrategy
    {
        public string Name => "Physical";
        public int Calculate(int power, int defense) =>
            Math.Max(1, power - defense);
    }

    public class MagicDamage : IDamageStrategy
    {
        public string Name => "Magic";
        public int Calculate(int power, int defense) =>
            (int)(power * 0.85);   // magic ignores most defense
    }

    public class CriticalHit : IDamageStrategy
    {
        public string Name => "Critical";
        public int Calculate(int power, int defense) =>
            (power - defense / 2) * 2;   // double after partial defense
    }

    // Context — uses whichever strategy is set
    public class Attacker
    {
        private IDamageStrategy _strategy;
        public string Name { get; }
        public int Power { get; }

        public Attacker(string name, int power, IDamageStrategy strategy)
        {
            Name = name;
            Power = power;
            _strategy = strategy;
        }

        // Swap at runtime — no recompilation needed
        public void SetStrategy(IDamageStrategy strategy) => _strategy = strategy;

        public int Attack(int targetDefense)
        {
            int dmg = _strategy.Calculate(Power, targetDefense);
            Console.WriteLine($"  {Name} uses {_strategy.Name} → {dmg} dmg");
            return dmg;
        }
    }

    // =========================================================
    //  3. FACTORY PATTERN (Simple Factory)
    //  A dedicated method/class creates objects so callers don't
    //  need to know the concrete types.
    //  Real uses: object pools, platform-specific code, plugins.
    //
    //  Participants:
    //    Product interface — what the factory produces
    //    Concrete products — specific implementations
    //    Factory           — the creator method/class
    // =========================================================
    public interface IEnemy
    {
        string Name { get; }
        int Health { get; }
        void Attack();
    }

    public class Goblin2 : IEnemy
    {
        public string Name => "Goblin";
        public int Health => 40;
        public void Attack() => Console.WriteLine("  Goblin stabs with a rusty dagger!");
    }

    public class Orc2 : IEnemy
    {
        public string Name => "Orc";
        public int Health => 120;
        public void Attack() => Console.WriteLine("  Orc smashes with a war axe!");
    }

    public class Dragon2 : IEnemy
    {
        public string Name => "Dragon";
        public int Health => 800;
        public void Attack() => Console.WriteLine("  Dragon breathes fire!");
    }

    // Factory — callers ask for an enemy by type, don't new() directly
    public static class EnemyFactory
    {
        public static IEnemy Create(string type) => type.ToLower() switch
        {
            "goblin" => new Goblin2(),
            "orc" => new Orc2(),
            "dragon" => new Dragon2(),
            _ => throw new ArgumentException($"Unknown enemy type: {type}")
        };

        // Can also create by difficulty — hides the decision logic
        public static IEnemy CreateForLevel(int level) => level switch
        {
            <= 3 => new Goblin2(),
            <= 7 => new Orc2(),
            _ => new Dragon2(),
        };
    }

    // =========================================================
    //  4. COMMAND PATTERN
    //  Encapsulate a REQUEST as an object so you can:
    //    • Queue commands
    //    • Log them
    //    • Undo / Redo them
    //  Real uses: undo stacks, macro recording, task queues.
    //
    //  Participants:
    //    ICommand    — Execute() and Undo()
    //    Receiver    — the object being acted on
    //    Invoker     — holds and executes commands
    // =========================================================
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    // Receiver — the thing being changed
    public class TextDocument
    {
        private string _text = "";
        public string Text => _text;

        public void Insert(string s) { _text += s; }
        public void Delete(int count)
        {
            if (count > _text.Length) count = _text.Length;
            _text = _text[..^count];
        }
    }

    // Concrete commands
    public class TypeCommand : ICommand
    {
        private readonly TextDocument _doc;
        private readonly string _text;
        public TypeCommand(TextDocument doc, string text) { _doc = doc; _text = text; }
        public void Execute() { _doc.Insert(_text); Console.WriteLine($"  Type: '{_text}'"); }
        public void Undo() { _doc.Delete(_text.Length); Console.WriteLine($"  Undo type: '{_text}'"); }
    }

    public class DeleteCommand : ICommand
    {
        private readonly TextDocument _doc;
        private readonly int _count;
        private string _deleted = "";
        public DeleteCommand(TextDocument doc, int count) { _doc = doc; _count = count; }

        public void Execute()
        {
            _deleted = _doc.Text.Length >= _count
                ? _doc.Text[^_count..] : _doc.Text;
            _doc.Delete(_count);
            Console.WriteLine($"  Delete {_count} chars (was: '{_deleted}')");
        }

        public void Undo()
        {
            _doc.Insert(_deleted);
            Console.WriteLine($"  Undo delete — restored: '{_deleted}'");
        }
    }

    // Invoker — holds history, enables undo
    public class CommandHistory
    {
        private readonly Stack<ICommand> _history = new();

        public void Execute(ICommand cmd)
        {
            cmd.Execute();
            _history.Push(cmd);
        }

        public void Undo()
        {
            if (_history.Count == 0) { Console.WriteLine("  Nothing to undo."); return; }
            _history.Pop().Undo();
        }

        public int HistoryDepth => _history.Count;
    }

    // =========================================================
    //  Demo runner
    // =========================================================
    public static class DesignPatternsDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Design Patterns ===\n");

            ObserverDemo();
            StrategyDemo();
            FactoryDemo();
            CommandDemo();
            PatternCheatSheet();
        }

        private static void ObserverDemo()
        {
            Console.WriteLine("--- 1. OBSERVER — notify many from one source ---\n");

            var game = new ObservableGame();
            game.Subscribe(new HudDisplay());
            game.Subscribe(new AchievementSystem());
            game.Subscribe(new AudioManager());

            Console.WriteLine("  Adding 300 points:");
            game.AddScore(300);
            Console.WriteLine("\n  Adding 400 more (triggers level-up at 500):");
            game.AddScore(400);
            Console.WriteLine("\n  Adding 600 more (triggers next level at 1000):");
            game.AddScore(600);
        }

        private static void StrategyDemo()
        {
            Console.WriteLine("\n--- 2. STRATEGY — swap algorithm at runtime ---\n");

            int targetDefense = 20;
            Console.WriteLine($"  Target defense = {targetDefense}\n");

            var hero = new Attacker("Hero", 80, new PhysicalDamage());
            hero.Attack(targetDefense);

            hero.SetStrategy(new MagicDamage());     // swap — no new class needed
            hero.Attack(targetDefense);

            hero.SetStrategy(new CriticalHit());
            hero.Attack(targetDefense);

            Console.WriteLine("\n  Same attacker, same target — three different strategies.");
        }

        private static void FactoryDemo()
        {
            Console.WriteLine("\n--- 3. FACTORY — decouple creation from usage ---\n");

            // Caller asks for a type by name — doesn't know or care about Goblin2/Orc2/Dragon2
            string[] wave = { "goblin", "orc", "goblin", "dragon" };
            Console.WriteLine("  Spawning wave by type:");
            foreach (string type in wave)
            {
                IEnemy e = EnemyFactory.Create(type);
                Console.Write($"  {e.Name} (HP={e.Health}) → ");
                e.Attack();
            }

            Console.WriteLine("\n  Spawning by level:");
            foreach (int lvl in new[] { 1, 5, 10 })
            {
                IEnemy e = EnemyFactory.CreateForLevel(lvl);
                Console.WriteLine($"  Level {lvl,2} → {e.Name}");
            }
        }

        private static void CommandDemo()
        {
            Console.WriteLine("\n--- 4. COMMAND — encapsulate actions, enable undo ---\n");

            var doc = new TextDocument();
            var history = new CommandHistory();

            history.Execute(new TypeCommand(doc, "Hello"));
            history.Execute(new TypeCommand(doc, ", World"));
            history.Execute(new TypeCommand(doc, "!"));
            Console.WriteLine($"  Doc: \"{doc.Text}\"");

            history.Execute(new DeleteCommand(doc, 6));   // delete "World!"
            Console.WriteLine($"  Doc: \"{doc.Text}\"");

            Console.WriteLine("\n  Undoing last 3 actions:");
            history.Undo();
            Console.WriteLine($"  Doc: \"{doc.Text}\"");
            history.Undo();
            Console.WriteLine($"  Doc: \"{doc.Text}\"");
            history.Undo();
            Console.WriteLine($"  Doc: \"{doc.Text}\"");
        }

        private static void PatternCheatSheet()
        {
            Console.WriteLine("\n--- Pattern Cheat Sheet ---\n");

            var patterns = new[]
            {
                ("Singleton",  "Creational",   "One instance, global access point"),
                ("Factory",    "Creational",   "Create objects without knowing concrete type"),
                ("Observer",   "Behavioural",  "Notify many listeners on state change"),
                ("Strategy",   "Behavioural",  "Swap algorithm/behaviour at runtime"),
                ("Command",    "Behavioural",  "Encapsulate action as object; enables undo"),
                ("Decorator",  "Structural",   "Add behaviour to object without changing class"),
                ("Adapter",    "Structural",   "Make incompatible interfaces work together"),
            };

            Console.WriteLine($"  {"Pattern",-12} {"Category",-14} {"One-liner"}");
            Console.WriteLine("  " + new string('-', 65));
            foreach (var (name, cat, desc) in patterns)
                Console.WriteLine($"  {name,-12} {cat,-14} {desc}");

            Console.WriteLine("\n  Rule of thumb for choosing:");
            Console.WriteLine("  'Need exactly one instance?'          → Singleton");
            Console.WriteLine("  'Creating objects from config/type?'  → Factory");
            Console.WriteLine("  'Many things must react to changes?'  → Observer (or Event)");
            Console.WriteLine("  'Behaviour varies and must swap?'     → Strategy");
            Console.WriteLine("  'Need undo or queueable actions?'     → Command");
        }
    }
}
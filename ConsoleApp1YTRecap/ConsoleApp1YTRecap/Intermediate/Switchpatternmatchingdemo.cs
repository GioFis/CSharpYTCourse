// ============================================================
//  SwitchPatternMatchingDemo.cs  —  EP.10  Switch Pattern Matching
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  Classic switch only matches CONSTANTS (ints, strings, enums).
//  Pattern matching extends switch to match TYPES, PROPERTIES,
//  RANGES, and arbitrary CONDITIONS — all in a clean, readable syntax.
//
//  Patterns covered:
//    1. switch EXPRESSION     — returns a value (=> syntax), no break needed
//    2. Type pattern          — case Car c:  /  c when c is Car
//    3. Property pattern      — { PropertyName: value/pattern }
//    4. Relational pattern    — < > <= >= inside a switch
//    5. Logical pattern       — and / or / not combinators
//    6. Positional pattern    — deconstruct a tuple or record
//    7. var pattern           — catch-all that names the value
//    8. Guard clause (when)   — add extra condition to any arm
//
//  switch EXPRESSION vs switch STATEMENT:
//    Expression → produces a value, arms use `=>`, no `break`
//    Statement  → traditional, uses `case:` / `break`
//    Prefer the expression form for modern C# — more concise.
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    // ── Types used in examples ────────────────────────────────────────────

    public abstract class Animal
    {
        public string Name { get; init; }
        public int Weight { get; init; }    // kg
        protected Animal(string name, int weight) { Name = name; Weight = weight; }
    }

    public class Dog : Animal
    {
        public bool IsGuard { get; init; }
        public Dog(string name, int w, bool guard = false)
            : base(name, w) { IsGuard = guard; }
    }
    public class Cat : Animal
    {
        public bool IsIndoor { get; init; }
        public Cat(string name, int w, bool indoor = true)
            : base(name, w) { IsIndoor = indoor; }
    }
    public class Bird : Animal
    {
        public double WingspanCm { get; init; }
        public Bird(string name, int w, double ws)
            : base(name, w) { WingspanCm = ws; }
    }
    public class Snake : Animal
    {
        public bool IsVenomous { get; init; }
        public Snake(string name, int w, bool venom)
            : base(name, w) { IsVenomous = venom; }
    }

    // A record — supports positional deconstruction natively
    public record Point2D(double X, double Y);

    public static class SwitchPatternMatchingDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Switch Pattern Matching ===\n");

            SwitchExpressionBasic();
            TypePatternDemo();
            PropertyPatternDemo();
            RelationalAndLogicalDemo();
            PositionalPatternDemo();
            GuardClauseDemo();
            RealWorldExample();
        }

        // ── 1. switch EXPRESSION — returns a value ─────────────────────
        // Clean alternative to if/else chains that produce a value.
        // No case/break — each arm is `pattern => value`.
        private static void SwitchExpressionBasic()
        {
            Console.WriteLine("--- 1. switch Expression (returns a value) ---\n");

            int[] scores = { 95, 83, 74, 61, 55, 40 };

            foreach (int score in scores)
            {
                // switch EXPRESSION — produces a string
                string grade = score switch
                {
                    >= 90 => "A",
                    >= 80 => "B",
                    >= 70 => "C",
                    >= 60 => "D",
                    _ => "F"   // _ is the default / discard arm
                };

                Console.WriteLine($"  Score {score,3} → Grade {grade}");
            }

            Console.WriteLine();

            // Works on any type — here an enum
            DayOfWeek today = DayOfWeek.Saturday;
            string dayType = today switch
            {
                DayOfWeek.Saturday or DayOfWeek.Sunday => "Weekend 🎉",
                DayOfWeek.Monday => "Ugh, Monday...",
                _ => "Weekday"
            };
            Console.WriteLine($"  {today} → {dayType}");
        }

        // ── 2. Type pattern — match on concrete type ──────────────────────
        private static void TypePatternDemo()
        {
            Console.WriteLine("\n--- 2. Type Pattern ---");
            Console.WriteLine("  Each arm matches a different concrete type.\n");

            var animals = new List<Animal>
            {
                new Dog("Rex",    30, guard: true),
                new Cat("Miso",   4,  indoor: false),
                new Bird("Tweety", 1, ws: 25),
                new Snake("Viper", 3, venom: true),
                new Dog("Buddy",  20),
            };

            foreach (Animal a in animals)
            {
                // Type pattern: each arm declares a typed variable
                string desc = a switch
                {
                    Dog d => $"Dog:   {d.Name} — guard={d.IsGuard}",
                    Cat c => $"Cat:   {c.Name} — indoor={c.IsIndoor}",
                    Bird b => $"Bird:  {b.Name} — wingspan={b.WingspanCm}cm",
                    Snake s => $"Snake: {s.Name} — venomous={s.IsVenomous}",
                    _ => $"???:   {a.Name}"
                };
                Console.WriteLine($"  {desc}");
            }
        }

        // ── 3. Property pattern — match on property values ────────────────
        // { Property: pattern }  — nest inside any pattern
        private static void PropertyPatternDemo()
        {
            Console.WriteLine("\n--- 3. Property Pattern ---");
            Console.WriteLine("  Match based on the object's property values.\n");

            var animals = new List<Animal>
            {
                new Dog("Rex",   30, guard: true),
                new Dog("Buddy", 8,  guard: false),
                new Cat("Miso",  4,  indoor: false),
                new Cat("Luna",  5,  indoor: true),
                new Snake("Kaa", 6,  venom: false),
                new Snake("Asp", 2,  venom: true),
            };

            foreach (Animal a in animals)
            {
                string warning = a switch
                {
                    // Type + property combined
                    Dog { IsGuard: true } => $"⚠ {a.Name} is a guard dog — careful!",
                    Snake { IsVenomous: true } => $"☠ {a.Name} is venomous — danger!",
                    Cat { IsIndoor: false } => $"🐾 {a.Name} roams outside",
                    Dog d => $"🐶 {d.Name} is friendly",
                    _ => $"  {a.Name} — no special warning"
                };
                Console.WriteLine($"  {warning}");
            }
        }

        // ── 4. Relational + Logical patterns ─────────────────────────────
        // Relational: < > <= >=
        // Logical:    and  or  not  (lowercase keywords)
        private static void RelationalAndLogicalDemo()
        {
            Console.WriteLine("\n--- 4. Relational & Logical Patterns ---\n");

            int[] temps = { -15, -5, 5, 15, 25, 35, 45 };

            foreach (int t in temps)
            {
                string label = t switch
                {
                    < -10 => "🥶 Extreme cold",
                    >= -10 and < 0 => "❄ Freezing",
                    >= 0 and < 15 => "🌥 Cool",
                    >= 15 and < 28 => "☀ Comfortable",
                    >= 28 and < 38 => "🌡 Hot",
                    _ => "🔥 Extreme heat"
                };
                Console.WriteLine($"  {t,4}°C → {label}");
            }

            Console.WriteLine();

            // 'not' pattern
            var animals = new List<Animal>
            {
                new Dog("Rex", 30), new Cat("Miso", 4), new Bird("Rio", 1, 30)
            };

            Console.WriteLine("  not pattern — flag non-dogs:");
            foreach (var a in animals)
            {
                string tag = a is not Dog ? "  NOT a dog" : "  Dog";
                Console.WriteLine($"  {a.Name,-8} → {tag}");
            }
        }

        // ── 5. Positional pattern — deconstruct tuples / records ──────────
        private static void PositionalPatternDemo()
        {
            Console.WriteLine("\n--- 5. Positional Pattern (Tuple & Record deconstruction) ---\n");

            // Tuple positional match
            var states = new List<(bool isAdmin, bool isVerified)>
            {
                (true,  true),
                (true,  false),
                (false, true),
                (false, false),
            };

            foreach (var s in states)
            {
                string access = s switch
                {
                    (true, true) => "Full access",
                    (true, false) => "Admin but not verified",
                    (false, true) => "Verified user",
                    (false, false) => "Guest — no access",
                };
                Console.WriteLine($"  admin={s.isAdmin,-5} verified={s.isVerified,-5} → {access}");
            }

            Console.WriteLine();

            // Record positional — Point2D(X, Y)
            var points = new[] { new Point2D(0, 0), new Point2D(3, 0), new Point2D(0, -2), new Point2D(1, 1) };
            foreach (var p in points)
            {
                string quadrant = p switch
                {
                    (0, 0) => "Origin",
                    ( > 0, > 0) => "Quadrant I",
                    ( < 0, > 0) => "Quadrant II",
                    ( < 0, < 0) => "Quadrant III",
                    ( > 0, < 0) => "Quadrant IV",
                    ( > 0, 0) or ( < 0, 0) => "On X-axis",
                    _ => "On Y-axis",
                };
                Console.WriteLine($"  ({p.X,4}, {p.Y,4}) → {quadrant}");
            }
        }

        // ── 6. Guard clause — 'when' adds an extra condition ─────────────
        // Use a switch STATEMENT with 'when' for complex guards.
        private static void GuardClauseDemo()
        {
            Console.WriteLine("\n--- 6. Guard Clause (when) in switch statement ---\n");

            var animals = new List<Animal>
            {
                new Dog("Tiny",   2, guard: true),
                new Dog("Rex",   40, guard: true),
                new Bird("Condor",  7, ws: 280),
                new Bird("Sparrow", 0, ws: 15),
            };

            foreach (Animal a in animals)
            {
                switch (a)
                {
                    case Dog d when d.Weight < 5:
                        Console.WriteLine($"  {d.Name} is a tiny guard dog 🐾");
                        break;
                    case Dog d when d.IsGuard:
                        Console.WriteLine($"  {d.Name} is a large guard dog ⚠");
                        break;
                    case Bird b when b.WingspanCm > 100:
                        Console.WriteLine($"  {b.Name} has a HUGE wingspan ({b.WingspanCm}cm)!");
                        break;
                    case Bird b:
                        Console.WriteLine($"  {b.Name} is a small bird ({b.WingspanCm}cm)");
                        break;
                }
            }
        }

        // ── 7. Real-world example: command dispatcher ─────────────────────
        private static void RealWorldExample()
        {
            Console.WriteLine("\n--- 7. Real-world: Command Dispatcher ---");
            Console.WriteLine("  Switch on tuple (command, role) — no if/else chain needed.\n");

            var commands = new (string cmd, string role)[]
            {
                ("delete", "admin"),
                ("delete", "user"),
                ("read",   "user"),
                ("write",  "editor"),
                ("write",  "guest"),
                ("deploy", "admin"),
                ("deploy", "editor"),
            };

            foreach (var (cmd, role) in commands)
            {
                string result = (cmd, role) switch
                {
                    ("delete", "admin") => "✓ Deleted",
                    ("delete", _) => "✗ Insufficient rights to delete",
                    ("read", _) => "✓ Read access granted",
                    ("write", "admin" or "editor") => "✓ Write access granted",
                    ("write", _) => "✗ Read-only account",
                    ("deploy", "admin") => "✓ Deploying to production",
                    ("deploy", _) => "✗ Only admins can deploy",
                    _ => "✗ Unknown command"
                };
                Console.WriteLine($"  {role,-8} cmd={cmd,-7} → {result}");
            }
        }
    }
}
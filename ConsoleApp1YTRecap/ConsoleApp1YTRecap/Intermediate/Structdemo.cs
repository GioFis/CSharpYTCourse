// ============================================================
//  StructDemo.cs  —  EP.10  Struct
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  A struct is a VALUE TYPE you define yourself.
//  It works like a class but is copied on assignment.
//
//  struct vs class — the core distinction:
//    struct → VALUE type → lives on the STACK (usually)
//             copied on assignment / method call
//             no inheritance (can implement interfaces)
//             no null (unless Nullable<T>)
//
//    class  → REFERENCE type → lives on the HEAP
//             reference copied on assignment
//             full inheritance
//             can be null
//
//  When to use struct:
//    ✓ Small, fixed data: Point, Color, Vector2, Size, Range
//    ✓ Logically a single value — not an "entity with identity"
//    ✓ Created/destroyed very frequently (avoids GC pressure)
//    ✓ Will be stored in large arrays (no pointer indirection)
//
//  When NOT to use struct:
//    ✗ Large data (>16 bytes) — copying gets expensive
//    ✗ You need inheritance
//    ✗ The struct will be mutated often (confusing copy semantics)
//    ✗ You need null to mean "absent" (use class or Nullable<T>)
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    // ── 1. Basic struct — 2D point ────────────────────────────────────────
    // Immutable by convention: fields are readonly, operations return new structs.
    public struct Point
    {
        // readonly fields — set only in constructor
        public readonly float X;
        public readonly float Y;

        public Point(float x, float y) { X = x; Y = y; }

        // Computed property — no backing field
        public float Length => MathF.Sqrt(X * X + Y * Y);

        // Operations return NEW structs (immutable pattern)
        public Point Add(Point other) => new Point(X + other.X, Y + other.Y);
        public Point Scale(float factor) => new Point(X * factor, Y * factor);

        // Distance to another point
        public float DistanceTo(Point other)
        {
            float dx = X - other.X;
            float dy = Y - other.Y;
            return MathF.Sqrt(dx * dx + dy * dy);
        }

        // Override ToString so Console.WriteLine looks nice
        public override string ToString() => $"({X:F2}, {Y:F2})";
    }

    // ── 2. Color struct — shows byte-range validation ─────────────────────
    public struct RgbColor
    {
        public readonly byte R;
        public readonly byte G;
        public readonly byte B;

        public RgbColor(byte r, byte g, byte b) { R = r; G = g; B = b; }

        // Predefined colours as static readonly fields
        public static readonly RgbColor Red = new(255, 0, 0);
        public static readonly RgbColor Green = new(0, 255, 0);
        public static readonly RgbColor Blue = new(0, 0, 255);
        public static readonly RgbColor Black = new(0, 0, 0);
        public static readonly RgbColor White = new(255, 255, 255);

        // Blend two colours (average)
        public RgbColor Blend(RgbColor other) =>
            new((byte)((R + other.R) / 2),
                (byte)((G + other.G) / 2),
                (byte)((B + other.B) / 2));

        public string ToHex() => $"#{R:X2}{G:X2}{B:X2}";

        public override string ToString() => $"RGB({R}, {G}, {B}) {ToHex()}";
    }

    // ── 3. Mutable struct — shown to demonstrate the copying pitfall ───────
    // (deliberately mutable so we can show the "copy surprise")
    public struct Counter
    {
        public int Value;   // mutable field — unusual for struct, but instructive
        public void Increment() => Value++;
        public override string ToString() => $"Counter({Value})";
    }

    public static class StructDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Struct ===\n");

            PointDemo();
            ColorDemo();
            CopySemantics();
            StructInArray();
            StructVsClassComparison();
        }

        // ── Point demos ───────────────────────────────────────────────────
        private static void PointDemo()
        {
            Console.WriteLine("--- 1. Point struct (immutable) ---");

            var origin = new Point(0, 0);
            var a = new Point(3, 4);
            var b = new Point(6, 8);

            Console.WriteLine($"  origin = {origin}");
            Console.WriteLine($"  a      = {a}   length = {a.Length:F2}");
            Console.WriteLine($"  b      = {b}");

            // Operations return NEW points — originals unchanged
            Point sum = a.Add(b);
            Point scaled = a.Scale(2);

            Console.WriteLine($"\n  a + b       = {sum}");
            Console.WriteLine($"  a × 2       = {scaled}");
            Console.WriteLine($"  a unchanged = {a}");

            Console.WriteLine($"\n  Distance a → b = {a.DistanceTo(b):F2}");
            Console.WriteLine($"  Distance a → origin = {a.DistanceTo(origin):F2}");
        }

        // ── Color demos ───────────────────────────────────────────────────
        private static void ColorDemo()
        {
            Console.WriteLine("\n--- 2. RgbColor struct ---");

            Console.WriteLine($"  Red   = {RgbColor.Red}");
            Console.WriteLine($"  Green = {RgbColor.Green}");
            Console.WriteLine($"  Blue  = {RgbColor.Blue}");

            RgbColor blended = RgbColor.Red.Blend(RgbColor.Blue);
            Console.WriteLine($"\n  Red blended with Blue = {blended}");

            var custom = new RgbColor(120, 200, 50);
            Console.WriteLine($"  Custom = {custom}");
        }

        // ── THE KEY LESSON: struct copy semantics ─────────────────────────
        private static void CopySemantics()
        {
            Console.WriteLine("\n--- 3. Copy semantics — the crucial struct behaviour ---");

            // VALUE TYPE COPY: modifying c2 does NOT affect c1
            Console.WriteLine("  Point (struct):");
            var p1 = new Point(1, 2);
            var p2 = p1;           // full copy of the data
            // p2 is a completely independent Point — changing it won't affect p1
            Console.WriteLine($"  p1={p1}   p2={p2}  (same so far)");
            // (Point is immutable so we demonstrate with Counter below)

            // Mutable struct — shows the copy surprise
            Console.WriteLine("\n  Counter (mutable struct) — the copy surprise:");
            var c1 = new Counter();    // Value = 0
            var c2 = c1;               // c2 is a COPY of c1

            c2.Increment();            // increments c2's copy
            c2.Increment();

            Console.WriteLine($"  c1 = {c1}   (NOT affected — it's a separate copy)");
            Console.WriteLine($"  c2 = {c2}   (only c2 incremented)");

            // Compare to a class — if Counter were a class, c1.Value would also be 2
            Console.WriteLine("\n  If Counter were a class (reference type),");
            Console.WriteLine("  c1 and c2 would point at the SAME object → c1.Value = 2.");
            Console.WriteLine("  Because it's a struct → they are independent → c1.Value = 0.");

            // Passing struct to a method — method gets a copy
            Console.WriteLine("\n  Passing struct to a method (receives a copy):");
            var original = new Counter();
            DoubleCounter(original);   // operates on a copy
            Console.WriteLine($"  original after DoubleCounter() = {original}  (unchanged!)");
        }

        private static void DoubleCounter(Counter c)
        {
            c.Increment();
            c.Increment();
            Console.WriteLine($"  Inside method: {c}");
            // c is a local copy — changes die here
        }

        // ── Struct in arrays — performance benefit ────────────────────────
        private static void StructInArray()
        {
            Console.WriteLine("\n--- 4. Structs in arrays — contiguous memory ---");
            Console.WriteLine("  An array of struct stores data inline (contiguous).");
            Console.WriteLine("  An array of class stores references → heap objects scattered.");
            Console.WriteLine("  For large arrays struct is often faster due to cache locality.\n");

            // 1 000 points stored inline — no heap allocation per element
            var points = new Point[5];
            for (int i = 0; i < points.Length; i++)
                points[i] = new Point(i * 1.5f, i * 2.0f);

            foreach (var p in points)
                Console.WriteLine($"  {p}  length={p.Length:F2}");
        }

        // ── Side-by-side comparison ───────────────────────────────────────
        private static void StructVsClassComparison()
        {
            Console.WriteLine("\n--- 5. Struct vs Class — decision guide ---\n");

            var rows = new[]
            {
                ("Aspect",          "struct",                        "class"),
                ("Type category",   "Value type",                    "Reference type"),
                ("Memory",          "Stack (usually)",               "Heap"),
                ("Assignment",      "Copies the data",               "Copies the reference"),
                ("null allowed",    "No (use Nullable<T>)",          "Yes"),
                ("Inheritance",     "Cannot inherit/be inherited",   "Full inheritance"),
                ("Default value",   "All fields zeroed",             "null"),
                ("Best for",        "Small, data-only, many copies", "Entities with identity"),
                ("Examples",        "Point, Color, Vector, Size",    "Player, Order, GameEngine"),
            };

            Console.WriteLine($"  {"Aspect",-20} {"struct",-32} {"class"}");
            Console.WriteLine("  " + new string('-', 75));
            foreach (var (aspect, sv, cl) in rows.Skip(1))
                Console.WriteLine($"  {aspect,-20} {sv,-32} {cl}");

            Console.WriteLine("\n  Rule of thumb:");
            Console.WriteLine("  → Does it represent a SINGLE VALUE with no identity? → struct");
            Console.WriteLine("  → Is it an ENTITY that gets passed around and mutated? → class");
        }
    }
}
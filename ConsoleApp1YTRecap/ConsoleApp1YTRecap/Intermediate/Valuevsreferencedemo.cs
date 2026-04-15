// ============================================================
//  ValueVsReferenceDemo.cs  —  EP.10  Value Types vs Reference Types
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  This is one of the most important concepts in C#.
//  Getting it wrong causes subtle bugs that are hard to find.
//
//  VALUE TYPE  (int, double, bool, char, struct, enum)
//    • The variable CONTAINS the data directly
//    • Stored on the STACK (fast, auto-cleaned)
//    • Assignment COPIES the data — two independent values
//    • Changing one does NOT affect the other
//
//  REFERENCE TYPE  (class, string*, array, interface, delegate)
//    • The variable holds a REFERENCE (address) to data on the HEAP
//    • Assignment COPIES the reference — both variables point at
//      the SAME object in memory
//    • Changing through one variable IS visible through the other
//    • Garbage Collector cleans up heap memory automatically
//
//  * string is special: it IS a reference type but it BEHAVES
//    like a value type because strings are IMMUTABLE —
//    every "change" creates a brand-new string object.
//
//  The rule of thumb:
//    Primitive / small / fixed-size / no identity → value type
//    Complex / variable-size / identity matters   → reference type
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    // ── A simple class (reference type) ──────────────────────────────────
    public class PlayerRef
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public PlayerRef(string name, int score) { Name = name; Score = score; }
        public override string ToString() => $"PlayerRef({Name}, {Score})";
    }

    // ── A simple struct (value type) — defined at bottom of file ─────────
    // (see StructDemo.cs for deeper struct coverage)

    public static class ValueVsReferenceDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Value Types vs Reference Types ===\n");

            ValueTypeCopy();
            ReferenceTypeAlias();
            MethodPassingValue();
            MethodPassingReference();
            StringImmutability();
            RefKeyword();
            NullDifference();
        }

        // ── 1. VALUE TYPE — assignment copies the data ────────────────────
        private static void ValueTypeCopy()
        {
            Console.WriteLine("--- 1. Value Type: assignment COPIES the value ---");

            int a = 10;
            int b = a;      // b gets a COPY of 10

            b = 99;         // changing b does NOT touch a

            Console.WriteLine($"  a = {a}   (unchanged)");
            Console.WriteLine($"  b = {b}   (only b changed)");
            Console.WriteLine("  → Two independent boxes in memory.");
        }

        // ── 2. REFERENCE TYPE — assignment copies the REFERENCE ───────────
        private static void ReferenceTypeAlias()
        {
            Console.WriteLine("\n--- 2. Reference Type: assignment copies the REFERENCE ---");

            var player1 = new PlayerRef("Alice", 100);
            var player2 = player1;   // player2 points to the SAME object!

            Console.WriteLine($"  Before change: player1={player1}");
            Console.WriteLine($"  Before change: player2={player2}");

            player2.Score = 999;     // modifying through player2...
            Console.WriteLine($"\n  After player2.Score = 999:");
            Console.WriteLine($"  player1={player1}  ← also changed!");
            Console.WriteLine($"  player2={player2}");
            Console.WriteLine("  → Both variables point at the SAME object on the heap.");
        }

        // ── 3. Passing VALUE TYPE to a method ────────────────────────────
        // The method gets a COPY — the original is safe.
        private static void MethodPassingValue()
        {
            Console.WriteLine("\n--- 3. Passing Value Type to a method ---");
            Console.WriteLine("  Method receives a COPY. Original is untouched.\n");

            int health = 100;
            Console.WriteLine($"  Before TakeDamage: health = {health}");
            TakeDamage(health, 30);                          // passes a copy
            Console.WriteLine($"  After  TakeDamage: health = {health}  (unchanged!)");
        }

        private static void TakeDamage(int hp, int damage)
        {
            hp -= damage;   // only affects the local copy
            Console.WriteLine($"  Inside TakeDamage: hp = {hp}");
        }

        // ── 4. Passing REFERENCE TYPE to a method ────────────────────────
        // The method gets the reference — it can mutate the ORIGINAL object.
        private static void MethodPassingReference()
        {
            Console.WriteLine("\n--- 4. Passing Reference Type to a method ---");
            Console.WriteLine("  Method receives the REFERENCE. It can mutate the original.\n");

            var player = new PlayerRef("Bob", 50);
            Console.WriteLine($"  Before LevelUp: {player}");
            LevelUp(player);                                  // passes the reference
            Console.WriteLine($"  After  LevelUp: {player}  ← mutated by the method!");
        }

        private static void LevelUp(PlayerRef p)
        {
            p.Score += 500;   // mutates the ORIGINAL object on the heap
            p.Name += " (★)";
        }

        // ── 5. String immutability ────────────────────────────────────────
        // string is a reference type BUT every "change" creates a new object.
        // The original string is never modified — it's read-only (immutable).
        private static void StringImmutability()
        {
            Console.WriteLine("\n--- 5. string: reference type that BEHAVES like a value type ---");

            string s1 = "Hello";
            string s2 = s1;         // s2 points to same string object

            s2 = s2 + " World";     // creates a BRAND NEW string object
                                    // s2 now points to the new one
                                    // s1 still points to "Hello"

            Console.WriteLine($"  s1 = \"{s1}\"   (unchanged — strings are immutable)");
            Console.WriteLine($"  s2 = \"{s2}\"");
            Console.WriteLine("  → Every string operation returns a new string.");
            Console.WriteLine("     Use StringBuilder for many concatenations in a loop.");
        }

        // ── 6. ref keyword — pass value type BY REFERENCE ─────────────────
        // Lets a method modify the ORIGINAL value-type variable.
        private static void RefKeyword()
        {
            Console.WriteLine("\n--- 6. ref keyword: force pass-by-reference ---");

            int score = 0;
            Console.WriteLine($"  Before AddPoints: score = {score}");
            AddPoints(ref score, 50);   // pass by reference — method can change 'score'
            Console.WriteLine($"  After  AddPoints: score = {score}  ← modified by method");

            // out is similar but the method MUST assign before returning
            SplitName("Alice Smith", out string first, out string last);
            Console.WriteLine($"\n  SplitName out: first='{first}'  last='{last}'");
        }

        private static void AddPoints(ref int score, int points)
        {
            score += points;   // modifies the CALLER's variable
        }

        private static void SplitName(string fullName, out string first, out string last)
        {
            var parts = fullName.Split(' ');
            first = parts[0];
            last = parts.Length > 1 ? parts[1] : "";
        }

        // ── 7. null — only reference types can be null by default ─────────
        private static void NullDifference()
        {
            Console.WriteLine("\n--- 7. null: only reference types can be null (by default) ---");

            // Reference type: can be null
            PlayerRef p = null;
            Console.WriteLine($"  PlayerRef p = null → {p?.ToString() ?? "null"}");

            // Value type: CANNOT be null
            // int x = null;  // ← compile error!

            // Make value type nullable with '?'
            int? maybeInt = null;
            Console.WriteLine($"  int? maybeInt = null → HasValue={maybeInt.HasValue}");
            maybeInt = 42;
            Console.WriteLine($"  int? maybeInt = 42   → HasValue={maybeInt.HasValue}, Value={maybeInt.Value}");

            Console.WriteLine("\n--- Summary ---");
            Console.WriteLine("  Value type:  stored inline, copied on assignment, no null by default");
            Console.WriteLine("  Ref type:    stored on heap, reference copied, can be null");
            Console.WriteLine("  string:      ref type, but immutable → acts like value type");
            Console.WriteLine("  ref/out:     let methods modify the caller's value-type variable");
        }
    }
}
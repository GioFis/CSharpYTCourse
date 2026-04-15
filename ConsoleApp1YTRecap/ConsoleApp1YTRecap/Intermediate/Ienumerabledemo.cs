// ============================================================
//  IEnumerableDemo.cs  —  EP.10  IEnumerable
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  IEnumerable<T> is the BASE interface for ALL iterable
//  collections in C#. If a type implements it, you can use
//  foreach on it, pass it to LINQ, and treat it generically.
//
//  The interface has ONE method:
//    IEnumerator<T> GetEnumerator()
//  The enumerator knows: current item, move to next, reset.
//
//  Why care?
//    • List<T>, array, Queue<T>, Stack<T>, Dictionary values,
//      string — all implement IEnumerable<T>
//    • LINQ (.Where, .Select, .OrderBy …) works on ANY IEnumerable<T>
//    • Accept IEnumerable<T> in a method parameter → works with
//      ANY collection type the caller has
//    • yield return — build lazy sequences without storing all data
//
//  IEnumerable<T>  vs  IEnumerator<T>:
//    IEnumerable → the COLLECTION  ("I can give you an enumerator")
//    IEnumerator → the CURSOR      ("I walk through items one by one")
//
//  Lazy vs Eager:
//    IEnumerable is LAZY — items are produced on demand
//    ToList() / ToArray() is EAGER — materialises everything now
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    public static class IEnumerableDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 IEnumerable ===\n");

            AcceptingIEnumerable();
            YieldReturnDemo();
            LazyEvaluationDemo();
            CustomIteratorDemo();
            LinqPipelineDemo();
        }

        // ── 1. Accept IEnumerable<T> — works with any collection ─────────
        // If your method only READS items, accept IEnumerable<T> not List<T>.
        // This lets callers pass arrays, lists, queues, or custom sequences.
        private static void AcceptingIEnumerable()
        {
            Console.WriteLine("--- 1. Accepting IEnumerable<T> as parameter ---");
            Console.WriteLine("  PrintAll works with List, array, Queue — any iterable.\n");

            List<string> list = new() { "Alpha", "Beta", "Gamma" };
            string[] array = { "One", "Two", "Three" };
            Queue<string> queue = new(new[] { "First", "Second", "Third" });

            Console.WriteLine("  From List<string>:"); PrintAll(list);
            Console.WriteLine("  From string[]:"); PrintAll(array);
            Console.WriteLine("  From Queue<string>:"); PrintAll(queue);
        }

        private static void PrintAll(IEnumerable<string> items)
        {
            int i = 1;
            foreach (string item in items)
                Console.WriteLine($"    {i++}. {item}");
        }

        // ── 2. yield return — build a lazy sequence ───────────────────────
        // The method body is PAUSED between yields and resumed on next MoveNext().
        // No list is allocated — values flow one at a time.
        private static void YieldReturnDemo()
        {
            Console.WriteLine("\n--- 2. yield return — lazy sequence generator ---");

            Console.Write("  Even 0-20:        ");
            foreach (int n in EvenNumbers(0, 20)) Console.Write($"{n} ");
            Console.WriteLine();

            Console.Write("  Fibonacci ≤ 200:  ");
            foreach (int f in FibonacciSequence(200)) Console.Write($"{f} ");
            Console.WriteLine();

            Console.Write("  Countdown from 5: ");
            foreach (string s in Countdown(5)) Console.Write($"{s} ");
            Console.WriteLine();
        }

        private static IEnumerable<int> EvenNumbers(int from, int to)
        {
            for (int i = from; i <= to; i++)
                if (i % 2 == 0) yield return i;
        }

        private static IEnumerable<int> FibonacciSequence(int maxValue)
        {
            int a = 0, b = 1;
            while (a <= maxValue) { yield return a; (a, b) = (b, a + b); }
        }

        private static IEnumerable<string> Countdown(int from)
        {
            for (int i = from; i > 0; i--) yield return i.ToString();
            yield return "GO!";
        }

        // ── 3. Lazy evaluation — nothing runs until iterated ──────────────
        private static void LazyEvaluationDemo()
        {
            Console.WriteLine("\n--- 3. Lazy vs Eager evaluation ---");
            Console.WriteLine("  IEnumerable does NO work until you iterate.\n");

            // Nothing computed yet — just a pipeline description
            IEnumerable<int> lazyPipeline = NumbersWithSideEffect(1, 10)
                .Where(n => n % 2 == 0);

            Console.WriteLine("  Pipeline defined — no output yet.");
            Console.WriteLine("  Now iterating (Take 3 only):");
            foreach (int n in lazyPipeline.Take(3)) Console.Write($"{n} ");
            Console.WriteLine("\n  (stopped after 3 — remaining items never generated)");

            Console.WriteLine("\n  ToList() — eager: ALL items computed immediately:");
            List<int> eager = NumbersWithSideEffect(1, 5).ToList();
            Console.WriteLine($"\n  (ToList finished — count: {eager.Count})");
        }

        private static IEnumerable<int> NumbersWithSideEffect(int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                Console.Write($"[gen:{i}] ");   // shows WHEN each value is produced
                yield return i;
            }
        }

        // ── 4. Custom iterable class ──────────────────────────────────────
        private static void CustomIteratorDemo()
        {
            Console.WriteLine("\n--- 4. Custom class implementing IEnumerable<T> ---");

            var range = new NumberRange(1, 8);

            Console.Write("  foreach: ");
            foreach (int n in range) Console.Write($"{n} ");
            Console.WriteLine();
            Console.WriteLine($"  LINQ .Sum() = {range.Sum()}");
            Console.WriteLine($"  LINQ .Max() = {range.Max()}");
        }

        // ── 5. LINQ as IEnumerable pipeline ──────────────────────────────
        private static void LinqPipelineDemo()
        {
            Console.WriteLine("\n--- 5. LINQ chains on IEnumerable<T> ---");

            var products = new List<(string Name, string Category, decimal Price)>
            {
                ("Apple",    "Fruit",  1.20m), ("Banana",   "Fruit",  0.50m),
                ("Carrot",   "Veggie", 0.80m), ("Mango",    "Fruit",  2.50m),
                ("Broccoli", "Veggie", 1.50m), ("Grape",    "Fruit",  3.00m),
            };

            // All lazy — nothing runs until the foreach
            IEnumerable<string> result = products
                .Where(p => p.Category == "Fruit")
                .OrderBy(p => p.Price)
                .Take(3)
                .Select(p => $"{p.Name} ({p.Price:C})");

            Console.WriteLine("  Cheapest 3 fruits:");
            foreach (string s in result) Console.WriteLine($"    {s}");

            Console.WriteLine("\n  Key rules:");
            Console.WriteLine("  • Accept IEnumerable<T> in params → works with any collection");
            Console.WriteLine("  • yield return  → lazy, zero allocation until consumed");
            Console.WriteLine("  • LINQ chains   → each step is lazy; ToList() materialises");
        }
    }

    // ── Custom class that IS iterable ────────────────────────────────────
    public class NumberRange : IEnumerable<int>
    {
        private readonly int _from, _to;
        public NumberRange(int from, int to) { _from = from; _to = to; }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = _from; i <= _to; i++) yield return i;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
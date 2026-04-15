// ============================================================
//  ParamsDemo.cs  —  EP.10  Params
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  The `params` keyword lets a method accept a VARIABLE NUMBER
//  of arguments of the same type — without the caller having
//  to build an array manually.
//
//  Under the hood C# still creates an array; `params` is
//  purely a convenience for the CALLER.
//
//  Rules:
//    • Only ONE params parameter per method
//    • It must be the LAST parameter in the signature
//    • The caller can pass 0, 1, or many values — or an existing array
//    • Inside the method it behaves exactly like a normal array
//
//  When to use:
//    • Utility/helper methods called with varying argument counts
//    • Console.WriteLine-style APIs
//    • Avoiding method overloads just to handle different counts
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    public static class ParamsDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Params ===\n");

            BasicParams();
            ParamsWithOtherArgs();
            ParamsVsArray();
            RealWorldExamples();
        }

        // ── 1. Basic params ───────────────────────────────────────────────
        private static void BasicParams()
        {
            Console.WriteLine("--- 1. Basic params ---");
            Console.WriteLine("  Same method, called with 1, 3, or 5 arguments:\n");

            // Caller syntax: just comma-separate the values — no array needed
            Console.WriteLine($"  Sum(5)           = {Sum(5)}");
            Console.WriteLine($"  Sum(1, 2, 3)     = {Sum(1, 2, 3)}");
            Console.WriteLine($"  Sum(10, 20, 30, 40, 50) = {Sum(10, 20, 30, 40, 50)}");

            // Zero arguments also works
            Console.WriteLine($"  Sum()            = {Sum()}");

            // Existing array also works — no copy, passed directly
            int[] numbers = { 2, 4, 6, 8 };
            Console.WriteLine($"  Sum(array)       = {Sum(numbers)}");
        }

        // params parameter: must be last, declared as T[]
        private static int Sum(params int[] values)
        {
            // Inside the method: just a normal array
            int total = 0;
            foreach (int v in values)
                total += v;
            return total;
        }

        // ── 2. params alongside regular parameters ────────────────────────
        // Regular parameters come FIRST; params comes LAST.
        private static void ParamsWithOtherArgs()
        {
            Console.WriteLine("\n--- 2. params + regular parameters ---");
            Console.WriteLine("  The label comes first (fixed), then any number of scores:\n");

            PrintScores("Alice", 95, 87, 91);
            PrintScores("Bob", 72);
            PrintScores("Carlo", 88, 90, 85, 93, 79);
        }

        private static void PrintScores(string name, params int[] scores)
        {
            if (scores.Length == 0)
            {
                Console.WriteLine($"  {name}: no scores recorded");
                return;
            }
            double avg = (double)scores.Sum() / scores.Length;
            Console.WriteLine($"  {name}: [{string.Join(", ", scores)}]  avg={avg:F1}");
        }

        // ── 3. params vs explicit array — same result, different caller UX ──
        private static void ParamsVsArray()
        {
            Console.WriteLine("\n--- 3. params vs explicit array (same method, two calling styles) ---");

            // With params: natural, readable
            string joined1 = Join(" | ", "Alpha", "Beta", "Gamma");

            // With array: more verbose, but equally valid
            string[] parts = { "Alpha", "Beta", "Gamma" };
            string joined2 = Join(" | ", parts);

            Console.WriteLine($"  params style: {joined1}");
            Console.WriteLine($"  array  style: {joined2}");
            Console.WriteLine($"  Results equal: {joined1 == joined2}");
        }

        private static string Join(string separator, params string[] parts)
        {
            return string.Join(separator, parts);
        }

        // ── 4. Real-world examples ────────────────────────────────────────
        private static void RealWorldExamples()
        {
            Console.WriteLine("\n--- 4. Real-world uses ---");

            // Logger that accepts any number of context tags
            Log("App started");
            Log("User logged in", "auth", "user:42");
            Log("Payment failed", "payment", "stripe", "retry");

            Console.WriteLine();

            // Math utility — min of any number of values
            Console.WriteLine($"  Min(3, 1, 4, 1, 5, 9) = {Min(3, 1, 4, 1, 5, 9)}");
            Console.WriteLine($"  Max(3, 1, 4, 1, 5, 9) = {Max(3, 1, 4, 1, 5, 9)}");

            Console.WriteLine("\n  Key rule: params must be the LAST parameter.");
            Console.WriteLine("  Only ONE params per method signature.");
        }

        private static void Log(string message, params string[] tags)
        {
            string tagStr = tags.Length > 0 ? $" [{string.Join(", ", tags)}]" : "";
            Console.WriteLine($"  LOG: {message}{tagStr}");
        }

        private static int Min(params int[] values) => values.Min();
        private static int Max(params int[] values) => values.Max();
    }
}
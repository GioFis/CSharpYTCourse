// ============================================================
//  RecursionDemo.cs  —  EP.10  Recursion
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  Recursion = a method that calls ITSELF to solve a problem
//  by breaking it into a smaller version of the same problem.
//
//  Every recursive method MUST have:
//    1. BASE CASE   — the condition that STOPS the recursion
//                     (no base case = infinite loop = StackOverflowException)
//    2. RECURSIVE CASE — the call to itself with a SIMPLER input
//                        (must move toward the base case each time)
//
//  Mental model — think of a stack of trays:
//    Each method call adds a tray (stack frame) to the pile.
//    When the base case is hit the trays are removed one by one,
//    each returning its result to the tray below it.
//
//  When to use recursion:
//    • The problem is naturally self-similar (tree, folder, fractal)
//    • Depth-first traversal of nested structures
//    • Mathematical definitions that refer to themselves (factorial, Fibonacci)
//  When NOT to use it:
//    • Simple counting loops (use a for/while — less overhead)
//    • Very deep inputs (risk of StackOverflowException)
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    public static class RecursionDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Recursion ===\n");

            FactorialDemo();
            FibonacciDemo();
            CountdownDemo();
            FolderTreeDemo();
        }

        // ── 1. Factorial ──────────────────────────────────────────────────
        // Factorial(5) = 5 × 4 × 3 × 2 × 1 = 120
        // Definition:  n! = n × (n-1)!       ← recursive case
        //               0! = 1               ← base case
        private static void FactorialDemo()
        {
            Console.WriteLine("--- 1. Factorial ---");
            Console.WriteLine("  Factorial(n) = n × Factorial(n-1)");
            Console.WriteLine("  Base case:    Factorial(0) = 1\n");

            for (int n = 0; n <= 7; n++)
                Console.WriteLine($"  {n}! = {Factorial(n)}");
        }

        private static long Factorial(int n)
        {
            // BASE CASE — stop here, return a known value
            if (n <= 1) return 1;

            // RECURSIVE CASE — call self with a smaller problem
            // "I know n! = n × (n-1)!, so ask the smaller version"
            return n * Factorial(n - 1);

            // Call stack for Factorial(4):
            //   Factorial(4) → 4 × Factorial(3)
            //     Factorial(3) → 3 × Factorial(2)
            //       Factorial(2) → 2 × Factorial(1)
            //         Factorial(1) → returns 1   ← base case hit!
            //       ← 2 × 1 = 2
            //     ← 3 × 2 = 6
            //   ← 4 × 6 = 24
        }

        // ── 2. Fibonacci ──────────────────────────────────────────────────
        // Fibonacci sequence: 0, 1, 1, 2, 3, 5, 8, 13, 21 …
        // Fib(n) = Fib(n-1) + Fib(n-2)   ← TWO recursive calls!
        // Base cases: Fib(0)=0, Fib(1)=1
        //
        // ⚠ Naive recursion is exponential — Fib(40) makes ~300 million calls.
        //    Shown here for learning; in production use a loop or memoization.
        private static void FibonacciDemo()
        {
            Console.WriteLine("\n--- 2. Fibonacci ---");
            Console.WriteLine("  Fib(n) = Fib(n-1) + Fib(n-2)");
            Console.WriteLine("  Base:   Fib(0)=0, Fib(1)=1\n");

            Console.Write("  Sequence: ");
            for (int i = 0; i <= 10; i++)
                Console.Write($"{Fibonacci(i)} ");
            Console.WriteLine();

            Console.WriteLine("\n  ⚠  Two recursive calls per step = exponential growth.");
            Console.WriteLine("     For large n, prefer an iterative version.");
        }

        private static int Fibonacci(int n)
        {
            if (n <= 0) return 0;   // base case 1
            if (n == 1) return 1;   // base case 2
            return Fibonacci(n - 1) + Fibonacci(n - 2);  // recursive case
        }

        // ── 3. Countdown — tracing the call stack visually ────────────────
        // Simplest example to SEE the stack unfold and then fold back.
        private static void CountdownDemo()
        {
            Console.WriteLine("\n--- 3. Countdown (trace call stack) ---");
            Console.WriteLine("  Watch: GOING DOWN (recursive calls) then COMING BACK UP\n");
            Countdown(5);
        }

        private static void Countdown(int n)
        {
            if (n < 0)  // BASE CASE
            {
                Console.WriteLine("  → Reached base case (n < 0), unwinding...");
                return;
            }

            Console.WriteLine($"  Going down... n = {n}");
            Countdown(n - 1);                           // recursive call
            Console.WriteLine($"  Coming back up... n = {n}");  // runs AFTER the call returns
        }

        // ── 4. Folder tree — a naturally recursive structure ───────────────
        // A folder contains files AND sub-folders (which themselves contain
        // files and sub-folders…). Recursion mirrors the structure perfectly.
        private static void FolderTreeDemo()
        {
            Console.WriteLine("\n--- 4. Folder Tree ---");
            Console.WriteLine("  A folder can contain other folders — recursion is a natural fit.\n");

            // Build a simple in-memory tree
            var root = new Folder("Project")
            {
                Files = { "README.md", "Program.cs" },
                SubFolders =
                {
                    new Folder("Assets")
                    {
                        Files = { "logo.png", "icon.ico" }
                    },
                    new Folder("Src")
                    {
                        Files = { "Game.cs" },
                        SubFolders =
                        {
                            new Folder("Utils")
                            {
                                Files = { "MathHelper.cs", "Logger.cs" }
                            }
                        }
                    }
                }
            };

            PrintFolder(root, indent: 0);
        }

        // Recursively prints a folder and all its children
        private static void PrintFolder(Folder folder, int indent)
        {
            string pad = new string(' ', indent * 2);
            Console.WriteLine($"{pad}📁 {folder.Name}/");

            foreach (string file in folder.Files)
                Console.WriteLine($"{pad}  📄 {file}");

            // BASE CASE (implicit): no sub-folders → foreach body never runs → returns
            // RECURSIVE CASE: call self for each child, increasing indent
            foreach (Folder sub in folder.SubFolders)
                PrintFolder(sub, indent + 1);
        }

        // Simple model class used by the tree demo
        private class Folder
        {
            public string Name { get; }
            public List<string> Files { get; } = new();
            public List<Folder> SubFolders { get; } = new();
            public Folder(string name) => Name = name;
        }
    }
}
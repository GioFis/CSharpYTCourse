// ============================================================
//  NestedLoopsDemo.cs  —  EP.10  Nested Loops
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  A nested loop is a loop inside another loop.
//  The INNER loop completes ALL its iterations for every
//  single step of the OUTER loop.
//
//  Mental model:
//    Outer loop = rows    (how many lines down)
//    Inner loop = columns (how many steps across per line)
//
//  Cost: if outer runs N times and inner runs M times
//        → total iterations = N × M  (watch out for big grids!)
//
//  Classic uses:
//    • Print grids / tables
//    • Work with 2-D arrays
//    • Generate combinations of two sets
//    • Pattern printing (triangles, stars)
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    public static class NestedLoopsDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Nested Loops ===\n");

            MultiplicationTable();
            StarPatterns();
            CombinationPairs();
            GridSearch();
        }

        // ── 1. Multiplication table ───────────────────────────────────────
        // Classic intro example: outer = row factor, inner = column factor.
        private static void MultiplicationTable()
        {
            Console.WriteLine("--- 1. Multiplication Table (5×5) ---");
            Console.WriteLine("     Outer loop counts ROWS (1..5).");
            Console.WriteLine("     Inner loop counts COLS (1..5) for EACH row.\n");

            // Header row
            Console.Write("    ");
            for (int col = 1; col <= 5; col++)
                Console.Write($"{col,4}");
            Console.WriteLine();
            Console.WriteLine("    " + new string('-', 20));

            for (int row = 1; row <= 5; row++)          // ← OUTER: 5 iterations
            {
                Console.Write($" {row} |");
                for (int col = 1; col <= 5; col++)      // ← INNER: 5 iterations each time
                    Console.Write($"{row * col,4}");
                Console.WriteLine();
            }

            Console.WriteLine($"\nTotal iterations: 5 × 5 = {5 * 5}");
        }

        // ── 2. Star patterns ─────────────────────────────────────────────
        // Shows how changing the inner loop's range creates shapes.
        private static void StarPatterns()
        {
            Console.WriteLine("\n--- 2. Star Patterns ---");

            // Square
            Console.WriteLine("Square (outer=row, inner always 4 wide):");
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                    Console.Write("* ");
                Console.WriteLine();
            }

            // Right triangle — inner limit GROWS with the row
            Console.WriteLine("\nRight Triangle (inner limit = row + 1):");
            for (int row = 1; row <= 5; row++)
            {
                for (int col = 1; col <= row; col++)    // ← inner limit changes each row!
                    Console.Write("* ");
                Console.WriteLine();
            }

            // Inverted triangle — inner limit SHRINKS with the row
            Console.WriteLine("\nInverted Triangle (inner limit = maxRow - row):");
            int maxRow = 5;
            for (int row = 0; row < maxRow; row++)
            {
                for (int col = 0; col < maxRow - row; col++)
                    Console.Write("* ");
                Console.WriteLine();
            }
        }

        // ── 3. Generating all pairs (combinations) ────────────────────────
        // Nested loops naturally produce every combination of two sequences.
        private static void CombinationPairs()
        {
            Console.WriteLine("\n--- 3. All Combinations of Two Sets ---");
            Console.WriteLine("Colors × Sizes — outer × inner produce every pair:\n");

            string[] colors = { "Red", "Blue", "Green" };
            string[] sizes = { "S", "M", "L" };

            int count = 0;
            foreach (string color in colors)            // ← OUTER
            {
                foreach (string size in sizes)          // ← INNER: runs fully for each color
                {
                    count++;
                    Console.WriteLine($"  {count,2}. {color}-{size}");
                }
            }

            Console.WriteLine($"\nTotal combinations: {colors.Length} × {sizes.Length} = {count}");
        }

        // ── 4. Grid search ────────────────────────────────────────────────
        // Scanning a 2-D array requires nested loops: one per dimension.
        private static void GridSearch()
        {
            Console.WriteLine("\n--- 4. Searching a 2-D Grid ---");

            int[,] grid = {
                { 3, 8,  1, 5 },
                { 7, 2,  9, 4 },
                { 6, 11, 0, 3 }
            };

            int target = 9;
            bool found = false;

            Console.WriteLine($"Looking for {target} in the grid...");

            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    if (grid[row, col] == target)
                    {
                        Console.WriteLine($"  Found {target} at row={row}, col={col}!");
                        found = true;
                        // In a real search you'd break here;
                        // shown without break so the scan is visible.
                    }
                }
            }

            if (!found) Console.WriteLine("  Not found.");

            // Show the grid so the position makes sense
            Console.WriteLine("\nGrid:");
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                Console.Write("  [ ");
                for (int col = 0; col < grid.GetLength(1); col++)
                    Console.Write($"{grid[row, col],3}");
                Console.WriteLine(" ]");
            }

            Console.WriteLine("\nKey rule: every extra nesting level multiplies the cost.");
            Console.WriteLine("3 levels deep on 10-item loops = 10³ = 1 000 iterations.");
        }
    }
}
// ============================================================
//  MultiDimArraysDemo.cs  —  EP.10  Multi-Dimensional Arrays
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  C# has two kinds of multi-dimensional arrays:
//
//  A) Rectangular (true 2D): int[,]
//     - One contiguous block of memory
//     - Every row has the same number of columns
//     - Syntax: arr[row, col]
//
//  B) Jagged (array of arrays): int[][]
//     - Each row is its own independent array
//     - Rows can have different lengths
//     - Syntax: arr[row][col]
//
//  When to use which?
//    Rectangular → grids, matrices, spreadsheets, images
//    Jagged      → triangles, varying-length data, graph adjacency
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    public static class MultiDimArraysDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Multi-Dimensional Arrays ===\n");

            RectangularDemo();
            JaggedDemo();
            PracticalGridDemo();
        }

        // ─────────────────────────────────────────────────────────────────
        // A) RECTANGULAR ARRAY — int[rows, cols]
        // ─────────────────────────────────────────────────────────────────
        private static void RectangularDemo()
        {
            Console.WriteLine("--- Rectangular Array (int[,]) ---");

            // Declaration + initialization inline
            int[,] matrix = new int[3, 4];   // 3 rows, 4 columns, all zeroes

            // Fill with row*10 + col so we can see the coordinates
            for (int row = 0; row < matrix.GetLength(0); row++)
                for (int col = 0; col < matrix.GetLength(1); col++)
                    matrix[row, col] = row * 10 + col;

            PrintRectangular(matrix);

            // Initializer syntax (like a literal table)
            int[,] multiplication = {
                { 1, 2, 3 },
                { 2, 4, 6 },
                { 3, 6, 9 }
            };

            Console.WriteLine("\nMultiplication table (3×3):");
            PrintRectangular(multiplication);

            // Useful properties
            Console.WriteLine($"\nRows:    {matrix.GetLength(0)}");   // dimension 0
            Console.WriteLine($"Cols:    {matrix.GetLength(1)}");   // dimension 1
            Console.WriteLine($"Total:   {matrix.Length}");          // all elements
        }

        // ─────────────────────────────────────────────────────────────────
        // B) JAGGED ARRAY — int[][]
        // ─────────────────────────────────────────────────────────────────
        private static void JaggedDemo()
        {
            Console.WriteLine("\n--- Jagged Array (int[][]) ---");

            // Each row is allocated separately → rows can differ in length
            int[][] triangle = new int[4][];

            for (int row = 0; row < triangle.Length; row++)
            {
                triangle[row] = new int[row + 1];  // row 0 → 1 element, row 3 → 4 elements
                for (int col = 0; col <= row; col++)
                    triangle[row][col] = col + 1;
            }

            Console.WriteLine("Triangle (jagged):");
            for (int row = 0; row < triangle.Length; row++)
            {
                Console.Write("  ");
                for (int col = 0; col < triangle[row].Length; col++)
                    Console.Write($"{triangle[row][col]} ");
                Console.WriteLine();
            }

            // Contrast: row lengths are independent
            Console.WriteLine("\nRow lengths:");
            for (int i = 0; i < triangle.Length; i++)
                Console.WriteLine($"  Row {i}: {triangle[i].Length} elements");
        }

        // ─────────────────────────────────────────────────────────────────
        // C) PRACTICAL EXAMPLE — simple dungeon grid
        // ─────────────────────────────────────────────────────────────────
        private static void PracticalGridDemo()
        {
            Console.WriteLine("\n--- Practical: Dungeon Grid ---");

            // '.' = floor, '#' = wall, 'P' = player, 'E' = exit
            char[,] map = {
                { '#', '#', '#', '#', '#' },
                { '#', '.', '.', '.', '#' },
                { '#', '.', '#', '.', '#' },
                { '#', 'P', '.', 'E', '#' },
                { '#', '#', '#', '#', '#' }
            };

            Console.WriteLine("Map:");
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            for (int r = 0; r < rows; r++)
            {
                Console.Write("  ");
                for (int c = 0; c < cols; c++)
                    Console.Write(map[r, c] + " ");
                Console.WriteLine();
            }

            // Find player position
            (int pr, int pc) = FindChar(map, 'P');
            Console.WriteLine($"\nPlayer is at row={pr}, col={pc}");

            // Move player right
            if (pc + 1 < cols && map[pr, pc + 1] != '#')
            {
                map[pr, pc + 1] = 'P';
                map[pr, pc] = '.';
                Console.WriteLine("Moved right!");
            }

            // Check win
            (int er, int ec) = FindChar(map, 'E');
            (int newPr, int newPc) = FindChar(map, 'P');
            if (newPr == er && newPc == ec)
                Console.WriteLine("Reached the exit! You win!");
            else
                Console.WriteLine($"Player is now at row={newPr}, col={newPc}");
        }

        // ── Helpers ──────────────────────────────────────────────────────

        private static void PrintRectangular(int[,] arr)
        {
            for (int r = 0; r < arr.GetLength(0); r++)
            {
                Console.Write("  [ ");
                for (int c = 0; c < arr.GetLength(1); c++)
                    Console.Write($"{arr[r, c],3}");
                Console.WriteLine(" ]");
            }
        }

        private static (int row, int col) FindChar(char[,] grid, char target)
        {
            for (int r = 0; r < grid.GetLength(0); r++)
                for (int c = 0; c < grid.GetLength(1); c++)
                    if (grid[r, c] == target)
                        return (r, c);
            return (-1, -1);
        }
    }
}
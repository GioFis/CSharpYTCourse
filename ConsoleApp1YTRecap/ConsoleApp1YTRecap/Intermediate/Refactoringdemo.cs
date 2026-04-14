// ============================================================
//  RefactoringDemo.cs  —  EP.10  Refactoring
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  Refactoring = improving the structure of existing code
//  WITHOUT changing what it does. Same output, cleaner code.
//
//  Key ideas:
//    1. Extract Method  — pull repeated/complex logic into its own method
//    2. Meaningful names — variables and methods that read like English
//    3. Single Responsibility — each method does ONE thing
//    4. DRY (Don't Repeat Yourself) — no copy-pasted logic
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    public static class RefactoringDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Refactoring ===\n");

            // ── BEFORE: everything crammed into one block ──────────────
            Console.WriteLine("--- BEFORE refactoring (all logic in one place) ---");

            int[] b = { 4, 7, 2, 9, 1 };
            int bMax = b[0];
            for (int i = 1; i < b.Length; i++)
                if (b[i] > bMax) bMax = b[i];

            int bMin = b[0];
            for (int i = 1; i < b.Length; i++)
                if (b[i] < bMin) bMin = b[i];

            double bSum = 0;
            for (int i = 0; i < b.Length; i++) bSum += b[i];
            double bAvg = bSum / b.Length;

            Console.WriteLine($"Max={bMax}  Min={bMin}  Avg={bAvg:F1}");

            // ── AFTER: each concern lives in its own named method ───────
            Console.WriteLine("\n--- AFTER refactoring (extracted methods) ---");

            int[] numbers = { 4, 7, 2, 9, 1 };

            PrintArrayStats(numbers);

            // ── WHY IT MATTERS ──────────────────────────────────────────
            Console.WriteLine("\n--- Why refactoring helps ---");
            Console.WriteLine("  * Each method has ONE job → easier to test");
            Console.WriteLine("  * Names explain intent   → easier to read");
            Console.WriteLine("  * No repeated loops      → fix a bug once");
            Console.WriteLine("  * Caller code is a story → easier to change");

            // ── ANOTHER EXAMPLE: Meaningful Names ───────────────────────
            Console.WriteLine("\n--- Meaningful names example ---");

            // Bad: what does this do?
            int x = 86400;

            // Good: self-documenting
            const int SecondsPerDay = 86400;
            int daysOfData = 3;
            int totalSeconds = daysOfData * SecondsPerDay;
            Console.WriteLine($"{daysOfData} days = {totalSeconds:N0} seconds");
        }

        // ── Extracted, focused methods ───────────────────────────────────

        // One method now DESCRIBES what happens at the call site
        private static void PrintArrayStats(int[] numbers)
        {
            int max = FindMax(numbers);
            int min = FindMin(numbers);
            double avg = ComputeAverage(numbers);

            Console.WriteLine($"Max={max}  Min={min}  Avg={avg:F1}");
        }

        // Each helper does exactly ONE thing
        private static int FindMax(int[] numbers)
        {
            int max = numbers[0];
            foreach (int n in numbers)
                if (n > max) max = n;
            return max;
        }

        private static int FindMin(int[] numbers)
        {
            int min = numbers[0];
            foreach (int n in numbers)
                if (n < min) min = n;
            return min;
        }

        private static double ComputeAverage(int[] numbers)
        {
            double sum = 0;
            foreach (int n in numbers) sum += n;
            return sum / numbers.Length;
        }
    }
}
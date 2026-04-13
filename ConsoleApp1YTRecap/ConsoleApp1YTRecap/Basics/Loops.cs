// ============================================================
//  Loops.cs  —  EP.6  Loops in C#
//  Covers: for, foreach, while, do-while, break, continue
//  Based on: Code with Mosh — C# Basics for Beginners series
//
//  KEY IDEA: A loop lets you repeat a block of code
//  without copy-pasting it. Three questions to pick the right loop:
//    1. Do I know exactly how many times I need to repeat?  → for
//    2. Am I walking through every item in a collection?    → foreach
//    3. Do I keep going while a condition is true?          → while / do-while
// ============================================================

namespace ConsoleApp1YTRecap.Basics
{
    class Loops
    {
        public static void Run()
        {
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("║        EP.6 — Loops in C#            ║");
            Console.WriteLine("╚══════════════════════════════════════╝\n");

            DemoForLoop();
            DemoForeachLoop();
            DemoWhileLoop();
            DemoDoWhileLoop();
            DemoBreakAndContinue();
            DemoNestedLoops();
            DemoPracticalExamples();
        }

        // ────────────────────────────────────────────────────
        //  1. FOR LOOP
        //  Use when: you know the exact number of iterations.
        //
        //  Anatomy:
        //    for ( initializer ; condition ; iterator )
        //    {
        //        // body — runs while condition is true
        //    }
        //
        //  Execution order:
        //    1. initializer runs ONCE before the first iteration
        //    2. condition is checked — if false, loop ends immediately
        //    3. body runs
        //    4. iterator runs
        //    5. back to step 2
        // ────────────────────────────────────────────────────
        static void DemoForLoop()
        {
            Console.WriteLine("─── 1. FOR LOOP ───────────────────────\n");

            // Count from 1 to 5
            Console.WriteLine("Counting 1 to 5:");
            for (int i = 1; i <= 5; i++)
            {
                // i++ is shorthand for i = i + 1 (the iterator)
                Console.WriteLine($"  i = {i}");
            }

            // Count DOWN — iterator uses i-- instead of i++
            Console.WriteLine("\nCounting down 5 to 1:");
            for (int i = 5; i >= 1; i--)
            {
                Console.WriteLine($"  i = {i}");
            }

            // Step by 2 — iterator uses i += 2
            Console.WriteLine("\nEven numbers 0 to 10 (step 2):");
            for (int i = 0; i <= 10; i += 2)
            {
                Console.Write($"{i}  ");
            }
            Console.WriteLine("\n");

            // Loop over an array by index
            // Useful when you need the index value itself
            string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            Console.WriteLine("Days of the week (via index):");
            for (int i = 0; i < days.Length; i++)
            {
                // days.Length = 7 → indices 0..6
                Console.WriteLine($"  [{i}] {days[i]}");
            }

            Console.WriteLine();
        }

        // ────────────────────────────────────────────────────
        //  2. FOREACH LOOP
        //  Use when: you want every item in a collection,
        //            and you don't need the index.
        //
        //  Cleaner and safer than a for loop over arrays:
        //    - No off-by-one errors (no i < array.Length)
        //    - Read-only: you cannot modify the collection inside it
        //
        //  Syntax:
        //    foreach (var item in collection)
        //    {
        //        // item holds the current element
        //    }
        // ────────────────────────────────────────────────────
        static void DemoForeachLoop()
        {
            Console.WriteLine("─── 2. FOREACH LOOP ────────────────────\n");

            int[] numbers = { 3, 7, 2, 9, 4, 6, 1 };

            Console.WriteLine("Printing all numbers:");
            foreach (var n in numbers)
            {
                // 'var' lets the compiler infer the type (int here)
                Console.Write($"{n}  ");
            }
            Console.WriteLine();

            // Summing elements — a classic foreach pattern
            int sum = 0;
            foreach (var n in numbers)
            {
                sum += n;   // accumulate
            }
            Console.WriteLine($"\nSum of all elements: {sum}");
            Console.WriteLine($"Average: {(double)sum / numbers.Length:F2}");

            // Foreach over a string — a string is a sequence of chars
            string word = "CSharp";
            Console.Write("\nLetters in \"CSharp\": ");
            foreach (char c in word)
            {
                Console.Write($"{c} ");
            }
            Console.WriteLine("\n");
        }

        // ────────────────────────────────────────────────────
        //  3. WHILE LOOP
        //  Use when: the number of iterations is not known upfront;
        //            you keep looping while a condition holds true.
        //
        //  ⚠ DANGER: if the condition never becomes false you get
        //    an INFINITE LOOP → always make sure the condition can
        //    eventually become false.
        //
        //  Syntax:
        //    while (condition)   ← checked BEFORE each iteration
        //    {
        //        // body
        //    }
        // ────────────────────────────────────────────────────
        static void DemoWhileLoop()
        {
            Console.WriteLine("─── 3. WHILE LOOP ──────────────────────\n");

            // Basic while — same as for but more flexible
            Console.WriteLine("Counting 1 to 5 with while:");
            int i = 1;              // initializer (outside the loop)
            while (i <= 5)          // condition
            {
                Console.WriteLine($"  i = {i}");
                i++;                // iterator (inside the body!)
            }

            // Classic use-case: repeat until user enters valid input
            // (simulated here so we don't block the demo menu)
            Console.WriteLine("\nSimulating input validation with while:");
            int attempt = 0;
            int secret = 42;
            int[] guesses = { 10, 55, 42 };   // pretend user typed these

            while (attempt < guesses.Length && guesses[attempt] != secret)
            {
                Console.WriteLine($"  Guess {guesses[attempt]} — wrong, try again.");
                attempt++;
            }
            if (attempt < guesses.Length)
                Console.WriteLine($"  Guess {guesses[attempt]} — correct! 🎉");

            Console.WriteLine();
        }

        // ────────────────────────────────────────────────────
        //  4. DO-WHILE LOOP
        //  Like while, but the body runs AT LEAST ONCE because
        //  the condition is checked AFTER each iteration, not before.
        //
        //  When to prefer do-while over while:
        //    - "Show the menu, then ask again" — menu always appears once.
        //    - "Generate a random number, then check" — generate first.
        //
        //  Syntax:
        //    do
        //    {
        //        // body (always executes at least once)
        //    }
        //    while (condition);   ← semicolon required!
        // ────────────────────────────────────────────────────
        static void DemoDoWhileLoop()
        {
            Console.WriteLine("─── 4. DO-WHILE LOOP ───────────────────\n");

            // This is the classic menu pattern Mosh shows:
            // The menu body runs once, then repeats if user says "no".
            // Here we simulate it without blocking on Console.ReadLine().

            int count = 0;
            do
            {
                count++;
                Console.WriteLine($"  [do-while] Body executed (count = {count})");
                // Normally here you'd read user input and set a flag.
                // We'll stop after 3 iterations to keep the demo moving.
            }
            while (count < 3);

            Console.WriteLine("  Loop finished after 3 iterations.\n");

            // Illustrate the key difference: condition starts FALSE
            // → while would skip the body entirely
            // → do-while still runs the body once
            int x = 100;

            Console.WriteLine("Condition is false from the start (x=100, condition x<5):");

            Console.Write("  while loop runs: ");
            while (x < 5) { Console.Write("yes "); }   // never prints "yes"
            Console.WriteLine("0 times");

            Console.Write("  do-while loop runs: ");
            do { Console.Write("once "); } while (x < 5);   // prints "once"
            Console.WriteLine("← body ran despite false condition\n");
        }

        // ────────────────────────────────────────────────────
        //  5. BREAK AND CONTINUE
        //
        //  break    → exits the loop immediately (jumps past it)
        //  continue → skips the rest of THIS iteration and goes to
        //             the next one (back to the condition check)
        //
        //  Mosh's tip: use these sparingly — they can make code
        //  harder to read. Often a well-crafted condition is cleaner.
        // ────────────────────────────────────────────────────
        static void DemoBreakAndContinue()
        {
            Console.WriteLine("─── 5. BREAK & CONTINUE ────────────────\n");

            // BREAK — stop as soon as we find the number 5
            Console.WriteLine("break: stop when we hit 5");
            for (int i = 1; i <= 10; i++)
            {
                if (i == 5)
                {
                    Console.WriteLine($"  Found 5 — breaking out of loop.");
                    break;          // jumps to the line after the closing brace
                }
                Console.WriteLine($"  i = {i}");
            }
            // Execution resumes here after break

            // CONTINUE — skip even numbers, only print odds
            Console.WriteLine("\ncontinue: skip even numbers (print odds 1-10)");
            for (int i = 1; i <= 10; i++)
            {
                if (i % 2 == 0)
                    continue;       // skip the Console.WriteLine below for even i

                Console.Write($"{i}  ");
            }
            Console.WriteLine("\n");
        }

        // ────────────────────────────────────────────────────
        //  6. NESTED LOOPS
        //  A loop inside a loop. break/continue only affect
        //  the INNERMOST loop they live in.
        //
        //  Time complexity warning: two nested loops over n items
        //  → O(n²) iterations. Fine for small n, can be slow for big n.
        // ────────────────────────────────────────────────────
        static void DemoNestedLoops()
        {
            Console.WriteLine("─── 6. NESTED LOOPS ────────────────────\n");

            // Classic: multiplication table (3×3 excerpt)
            Console.WriteLine("3×3 multiplication table:");
            for (int row = 1; row <= 3; row++)
            {
                for (int col = 1; col <= 3; col++)
                {
                    Console.Write($"  {row}×{col}={row * col,-3}");   // -3 = left-align in 3 chars
                }
                Console.WriteLine();    // newline after each row
            }

            // Each inner loop runs 3 times per outer iteration:
            // outer 1 → inner 1,2,3
            // outer 2 → inner 1,2,3
            // outer 3 → inner 1,2,3   → 9 total iterations

            Console.WriteLine();
        }

        // ────────────────────────────────────────────────────
        //  7. PRACTICAL EXAMPLES
        //  Real-world patterns Mosh demonstrates:
        //    a) Sum of digits in a number
        //    b) Reverse a number
        //    c) Find the largest number in an array
        // ────────────────────────────────────────────────────
        static void DemoPracticalExamples()
        {
            Console.WriteLine("─── 7. PRACTICAL EXAMPLES ──────────────\n");

            // ── a) Sum of digits ──────────────────────────────
            // Algorithm: repeatedly strip the last digit with % 10,
            // then remove it with / 10.  Loop while number > 0.
            int number = 1357;
            int sumOfDigits = 0;
            int temp = number;

            while (temp > 0)
            {
                sumOfDigits += temp % 10;   // grab last digit (e.g. 1357 % 10 = 7)
                temp /= 10;                 // remove last digit (e.g. 1357 / 10 = 135)
            }
            Console.WriteLine($"a) Sum of digits of {number}: {sumOfDigits}");
            // 1+3+5+7 = 16

            // ── b) Reverse a number ───────────────────────────
            int original = 1234;
            int reversed = 0;
            temp = original;

            while (temp > 0)
            {
                int digit = temp % 10;          // grab last digit
                reversed = reversed * 10 + digit; // shift left and append
                temp /= 10;
            }
            Console.WriteLine($"b) Reverse of {original}: {reversed}");

            // ── c) Largest number in array ────────────────────
            // Start by assuming the first element is the largest.
            // Walk through and update whenever we find something bigger.
            int[] scores = { 45, 78, 23, 91, 67, 55 };
            int largest = scores[0];            // assume first is largest

            foreach (var score in scores)
            {
                if (score > largest)
                    largest = score;            // found a new champion
            }
            Console.WriteLine($"c) Largest score in array: {largest}");

            // ── d) FizzBuzz (classic interview question) ───────
            // 1-20: print Fizz if divisible by 3, Buzz if by 5,
            //        FizzBuzz if by both, else the number itself.
            Console.WriteLine("\nd) FizzBuzz (1-20):");
            for (int i = 1; i <= 20; i++)
            {
                if (i % 3 == 0 && i % 5 == 0)
                    Console.Write("FizzBuzz ");
                else if (i % 3 == 0)
                    Console.Write("Fizz ");
                else if (i % 5 == 0)
                    Console.Write("Buzz ");
                else
                    Console.Write($"{i} ");
            }

            Console.WriteLine("\n");
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine("  SUMMARY");
            Console.WriteLine("  for      → known iteration count");
            Console.WriteLine("  foreach  → walk every item in a collection");
            Console.WriteLine("  while    → repeat while condition is true (check first)");
            Console.WriteLine("  do-while → same but body always runs at least once");
            Console.WriteLine("  break    → exit the loop immediately");
            Console.WriteLine("  continue → skip current iteration, go to next");
            Console.WriteLine("═══════════════════════════════════════\n");
        }
    }
}
// ============================================================
//  CollectionsDemo.cs  —  EP.2: Arrays, Loops, Enum, Switch
//  Enum is declared at namespace level (it's a TYPE definition).
// ============================================================

using System;
using System.Collections.Generic;

namespace ConsoleApp1YTRecap
{
    enum PlayerState { Attack, Defend }   // namespace-level type

    class CollectionsDemo
    {
        static PlayerState playerState = PlayerState.Defend;  // class-level field

        public static void Run()
        {
            Console.WriteLine("--- EP.2: Collections & Loops ---\n");

            // ARRAYS
            int[] intArray = new int[5];                     // fixed size, default 0
            Console.WriteLine("Fixed size, default 0");
            Console.WriteLine(intArray);
            int[] intArray2 = new int[] { 1, 2, 3, 4, 5 };   // initialized values
            List<int> intList = new List<int>() { 7, 8, 9 }; // dynamic, can grow

            // FOREACH — no index needed
            Console.WriteLine("foreach:");
            foreach (int item in intList)
                Console.WriteLine("  " + item);

            // FOR — index-based
            Console.WriteLine("for:");
            for (int i = 0; i < intArray.Length; i++)
                Console.WriteLine($"  [{i}] = {intArray[i]}");

            // WHILE — manual counter
            Console.WriteLine("while:");
            int j = 0;
            while (j < intArray2.Length)
            {
                Console.WriteLine($"  [{j}] = {intArray2[j]}");
                j++;
            }

            // SWITCH on enum
            Console.WriteLine("switch:");
            switch (playerState)
            {
                case PlayerState.Attack: Console.WriteLine("  Attacking!"); break;
                case PlayerState.Defend: Console.WriteLine("  Defending!"); break;
                default: Console.WriteLine("  Unknown."); break;
            }
        }
    }
}
// ============================================================
//  Program.cs  —  ENTRY POINT ONLY
//  This file does one job: start the program and route to demos.
//  All logic lives in separate files in the same namespace.
//  C# compiles every .cs file in the project automatically —
//  no imports needed, just the same namespace is enough.
// ============================================================

using TryPlay1;

namespace ConsoleApp1YTRecap
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== TryPlay1 — C# Learning Project ===\n");
            Console.WriteLine("Pick a demo:");
            Console.WriteLine("  1 - EP.1  Basics (types, operators, methods)");
            Console.WriteLine("  2 - EP.2  Arrays, Loops, Enum, Switch");
            Console.WriteLine("  3 - EP.3  Events & Delegates");
            Console.WriteLine("  4 - EP.4  Testing Events (Unity MonoBehaviour)");
            Console.Write("\nEnter 1, 2 or 3: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1": BasicsDemo.Run(); break;
                case "2": CollectionsDemo.Run(); break;
                case "3": EventsDemo.Run(); break;
                case "4": TestingEvents.Run(); break;
                default:
                    Console.WriteLine("Invalid choice. Restart and enter 1, 2 or 3.");
                    break;
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
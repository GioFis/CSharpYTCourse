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
            bool running = true;
            while (running)
            { 
            Console.WriteLine("Pick a demo:");
            Console.WriteLine("  1 - EP.1  Basics (types, operators, methods)");
            Console.WriteLine("  2 - EP.2  Arrays, Loops, Enum, Switch");
            Console.WriteLine("  3 - EP.3  Events & Delegates");
            Console.WriteLine("  4 - EP.4  Testing Events ");
            Console.WriteLine("  5 - EP.5  VideoEncoderDelegates");
            Console.Write("\nEnter 1, 2, 3, 4 or 5: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1": BasicsDemo.Run(); break;
                case "2": CollectionsDemo.Run(); break;
                case "3": EventsDemo.Run(); break;
                case "4": TestingEvents.Run(); break;
                case "5": VideoEncoder.Run(); break;
                case "Q":
                    running = false;
                    Console.WriteLine("Exiting the program. Goodbye!");
                    continue;
                default:
                    Console.WriteLine("Invalid choice. Restart and enter 1-5 or Q to quit.");
                    break;
            }
            if (running)
            {
                Console.WriteLine("\n Return to menu? (Y/N) ");
                string again = Console.ReadLine();
                if (again?.ToUpper() != "Y")
                {
                    running = false;
                    Console.WriteLine("Goodbye!");
                }
                Console.WriteLine();
            }
            }
        }
    }
}
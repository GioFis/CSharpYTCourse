// ============================================================
//  Program.cs  —  ENTRY POINT ONLY
//  This file does one job: start the program and route to demos.
//  All logic lives in separate files in the same namespace.
//  C# compiles every .cs file in the project automatically —
//  no imports needed, just the same namespace is enough.
//Is it "something happened and others need to know"? → Event.
//Is it "I need to treat different objects the same way based on what they can do"? → Interface.
//Is it "I'm writing logic that doesn't care about the specific type"? → Generic.
// ============================================================

using ConsoleApp1YTRecap.Basics;
using ConsoleApp1YTRecap.Events;
using ConsoleApp1YTRecap.Generics;
using ConsoleApp1YTRecap.Interfaces;
using ConsoleApp1YTRecap.Videos;
using ConsoleApp1YTRecap.Intermediate;
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
            Console.WriteLine("  6 - EP.6  Interfaces (IAttackable and TryInterFace)");
            Console.WriteLine("  7 - EP.7  Generics (GenericInventory Inventory<T> and TryGenerics)");
            Console.WriteLine("  8 - EP.8  Loops (for, foreach, while, do-while");
            Console.WriteLine("  9 - EP.9  Null Reference (nullable types, ?., ??, ??=)");
            Console.WriteLine("  10 - EP.10 Refactoring (extract methods, naming, DRY)");
            Console.WriteLine("  11 - EP.10 Enums (basic, flags, switch, parse)");
            Console.WriteLine("  12 - EP.10 Properties (auto, full, computed, init-only)");
            Console.WriteLine("  13 - EP.10 Multi-Dimensional Arrays (rectangular + jagged)");
            Console.WriteLine("  14 - EP.10 Nested Loops (grids, patterns, combinations)");
            Console.WriteLine("  15 - EP.10 Recursion (factorial, fibonacci, folder tree)");
            Console.WriteLine("  16 - EP.10 Dictionary (CRUD, TryGetValue, frequency, grouping)");
            Console.WriteLine("  17 - EP.10 Other Collections (HashSet, Stack, Queue, LinkedList)");
                Console.Write("\nEnter 1-13 or Q to quit: ");


                string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1": BasicsDemo.Run(); break;
                case "2": CollectionsDemo.Run(); break;
                case "3": EventsDemo.Run(); break;
                case "4": TestingEvents.Run(); break;
                case "5": VideoEncoder.Run(); break;
                case "6": TryInterface.Run(); break;
                case "7": TryGenerics.Run(); break;
                case "8": Loops.Run(); break;
                case "9": NullReference.Run(); break;
                case "10": RefactoringDemo.Run(); break;
                case "11": EnumsDemo.Run(); break;
                case "12": PropertiesDemo.Run(); break;
                case "13": MultiDimArraysDemo.Run(); break;
                case "14": NestedLoopsDemo.Run(); break;
                case "15": RecursionDemo.Run(); break;
                case "16": DictionaryDemo.Run(); break;
                case "17": OtherCollectionsDemo.Run(); break;
                case "Q":
                    running = false;
                    Console.WriteLine("Exiting the program. Goodbye!");
                    continue;
                default:
                    Console.WriteLine("Invalid choice. Enter 1-17 or Q to quit.");
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
// ============================================================
//  BasicsDemo.cs  —  EP.1: Types, Operators, Conditionals, Methods
//  Same namespace = Program.cs can call BasicsDemo.Run() freely.
//  No import needed.
// PUBLIC, PRIVATE ACCESS MODIFIERS:
// public and private are the two most common access modifier keywords in C#.
// They control the visibility and accessibility of classes, methods, and other members.
// Here's a brief explanation of each:
// 1. public: When a class, method, or member is declared as public, it can be accessed from anywhere in the code.
// This means that any other class or method can use it without any restrictions.
// 2. private: When a class, method, or member is declared as private, it can only be accessed within the same class.
// Static methods and members belong to the class itself rather than an instance of the class, so they can be accessed without creating an object of the class.
// They are often used for utility functions or shared data that doesn't depend on instance state. In this demo, all methods are static for simplicity,
// allowing us to call them directly from the Run() method without needing to create an instance of BasicsDemo.
// STATIC VS NON-STATIC METHODS:
// Non-static methods would require creating an object of the class to call them,
// which is not necessary for this basic demonstration of C# features.
// ============================================================

namespace ConsoleApp1YTRecap.Basics
{
    class BasicsDemo
    {
        public static void Run()
        {
            Console.WriteLine("--- EP.1: Basics ---\n");

            // DATA TYPES
            int i = 42;
            float f = 3.14f;
            double d = 2.71828;
            decimal m = 9.99m;
            string s = "Hello";
            char c = 'A';
            bool b = true;
            long l = 9876543210L;

            // CONSOLE METHODS
            Console.WriteLine("Hello, World!");
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine($"Hello {name}! i = {i}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("(cyan text example with the variables defined before)");
            Console.WriteLine(i.ToString() + " is the value of i");
            Console.WriteLine($"i = {i} (interpolated string example)");
            Console.WriteLine(f.ToString("F2") + " is f with 2 decimals");
            Console.WriteLine($"d = {d:F3} (interpolated with 3 decimals)");
            Console.WriteLine($"m = {m:C} (interpolated as currency)");
            Console.WriteLine($"c = '{c}' (char example)");
            Console.WriteLine($"b = {b} (boolean example)");
            Console.WriteLine($"l = {l:N0} (long with thousand separators)");
            Console.WriteLine(s.ToUpper() + " is s in uppercase");
            Console.ResetColor();

            // TYPE CONVERSION
            string input = "123";
            int parsed = int.Parse(input);          // string → int
            double conv = Convert.ToDouble(input);   // string → double
            string fromInt = i.ToString();              // int → string

            // OPERATORS
            int sum = i + 10;
            int diff = i - 10;
            int product = i * 2;
            int quotient = i / 5;
            int remainder = i % 5;
            i++; i--;

            // CONDITIONALS
            if (i > 10 && b)
                Console.WriteLine("i > 10 AND b is true");
            else if (i > 10 || !b)
                Console.WriteLine("i > 10 OR b is false");
            else
                Console.WriteLine("no match");

            // METHODS
            Greet();
            Console.WriteLine("IsSmall(i): " + IsSmall(i));
            Console.WriteLine("Add(3,4):   " + Add(3, 4));
        }

        static void Greet() { Console.WriteLine("Hello from Greet()!"); }
        static bool IsSmall(int x) { return x < 100; }
        static int Add(int a, int b) { return a + b; }
    }
}
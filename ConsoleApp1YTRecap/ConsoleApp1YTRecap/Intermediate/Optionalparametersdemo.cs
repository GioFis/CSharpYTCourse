// ============================================================
//  OptionalParametersDemo.cs  —  EP.10  Optional Parameters
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  Optional parameters have a DEFAULT VALUE in the signature.
//  The caller can omit them — the compiler substitutes the default.
//
//  Closely related: NAMED ARGUMENTS let the caller pass arguments
//  by name instead of position, which:
//    • Makes call sites self-documenting
//    • Lets you skip optional parameters in the middle
//
//  Rules:
//    • Optional parameters must come AFTER required ones
//    • Default values must be compile-time constants
//      (literals, const, enum members, null, default)
//    • Named args can appear in any order
//    • You can mix positional and named args (positional first)
//
//  Common use:
//    • Config/builder-style methods with many settings
//    • Replacing overload families that differ only in defaults
//    • Making APIs friendlier without breaking existing callers
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    public static class OptionalParametersDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Optional Parameters ===\n");

            BasicOptional();
            NamedArguments();
            OverloadsVsOptional();
            RealWorldConfig();
        }

        // ── 1. Basic optional parameters ─────────────────────────────────
        private static void BasicOptional()
        {
            Console.WriteLine("--- 1. Basic Optional Parameters ---");
            Console.WriteLine("  Signature: Greet(string name, string greeting = \"Hello\", bool shout = false)\n");

            // All three variants compile — the method signature handles the rest
            Greet("Alice");                        // uses both defaults
            Greet("Bob", "Hi");                    // overrides greeting, shout stays false
            Greet("Carlo", "Ciao", shout: true);   // overrides both (named for clarity)

            Console.WriteLine();

            // Numeric defaults
            Console.WriteLine($"  Power(2)        = {Power(2)}");        // exponent defaults to 2
            Console.WriteLine($"  Power(2, 10)    = {Power(2, 10)}");
            Console.WriteLine($"  Power(3)        = {Power(3)}");
        }

        private static void Greet(string name, string greeting = "Hello", bool shout = false)
        {
            string message = $"{greeting}, {name}!";
            Console.WriteLine($"  → {(shout ? message.ToUpper() : message)}");
        }

        private static double Power(double baseNum, int exponent = 2)
            => Math.Pow(baseNum, exponent);

        // ── 2. Named arguments ────────────────────────────────────────────
        // You can name ANY argument — optional or not — to make code clearer.
        private static void NamedArguments()
        {
            Console.WriteLine("--- 2. Named Arguments ---");
            Console.WriteLine("  Same method, different calling styles:\n");

            // Positional (order matters)
            CreateOrder("Laptop", 1299.99m, 2, true);

            // Named (order doesn't matter — skips to the arg you need)
            CreateOrder(
                productName: "Mouse",
                price: 29.99m,
                quantity: 5,
                express: false
            );

            // Mix: positional first, then named for the optional ones
            // Skip 'quantity' entirely by naming 'express' directly
            CreateOrder("Keyboard", 79.99m, express: true);

            Console.WriteLine();
            Console.WriteLine("  Named args shine when parameters are booleans or look ambiguous:");
            SendEmail("alice@example.com", isHtml: true, priority: 1);
        }

        private static void CreateOrder(string productName, decimal price,
                                        int quantity = 1, bool express = false)
        {
            string delivery = express ? "EXPRESS" : "standard";
            Console.WriteLine($"  Order: {productName} × {quantity} @ {price:C} [{delivery}]");
        }

        private static void SendEmail(string to, bool isHtml = false, int priority = 3)
        {
            Console.WriteLine($"  Email → {to}  html={isHtml}  priority={priority}");
        }

        // ── 3. Overloads vs optional parameters ───────────────────────────
        // Before optional params, you'd write multiple overloads.
        // Optional params collapse them into one — less code, same flexibility.
        private static void OverloadsVsOptional()
        {
            Console.WriteLine("\n--- 3. Overloads vs Optional Parameters ---");

            Console.WriteLine("  OLD WAY — three separate overloads:");
            Console.WriteLine("    DrawBox(width)");
            Console.WriteLine("    DrawBox(width, height)");
            Console.WriteLine("    DrawBox(width, height, symbol)");

            Console.WriteLine("\n  NEW WAY — one method, optional params:");
            DrawBox(6);                 // height=3, symbol='*'
            DrawBox(8, 2);             // symbol='*'
            DrawBox(5, 3, '#');        // all specified
        }

        // One method replaces three overloads
        private static void DrawBox(int width, int height = 3, char symbol = '*')
        {
            Console.WriteLine($"\n  DrawBox({width}, {height}, '{symbol}'):");
            for (int r = 0; r < height; r++)
            {
                Console.Write("  ");
                for (int c = 0; c < width; c++)
                    Console.Write(symbol);
                Console.WriteLine();
            }
        }

        // ── 4. Real-world config-style method ─────────────────────────────
        // A method with many settings — callers only specify what they care about.
        private static void RealWorldConfig()
        {
            Console.WriteLine("\n--- 4. Real-world: Config / Builder style ---");

            // Minimal call — all defaults
            StartServer();

            // Override just what's needed, by name
            StartServer(port: 8080, maxConnections: 100);

            // Full specification
            StartServer(host: "0.0.0.0", port: 443, enableSsl: true,
                        maxConnections: 500, timeout: 60);

            Console.WriteLine("\n  Tip: use named args when you pass more than 2-3 args,");
            Console.WriteLine("  or when boolean/int args look ambiguous at the call site.");
        }

        private static void StartServer(
            string host = "localhost",
            int port = 3000,
            bool enableSsl = false,
            int maxConnections = 50,
            int timeout = 30)
        {
            string scheme = enableSsl ? "https" : "http";
            Console.WriteLine($"\n  Server → {scheme}://{host}:{port}");
            Console.WriteLine($"           maxConn={maxConnections}  timeout={timeout}s");
        }
    }
}
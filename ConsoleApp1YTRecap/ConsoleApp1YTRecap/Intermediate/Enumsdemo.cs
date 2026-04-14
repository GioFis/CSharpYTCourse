// ============================================================
//  EnumsDemo.cs  —  EP.10  Enums
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  An enum is a named set of related integer constants.
//  Use it whenever a variable should only hold one of a
//  fixed set of meaningful values (direction, status, tier…).
//
//  Key ideas:
//    1. Basic enum — replaces magic numbers with readable names
//    2. Underlying value — each member is secretly an int
//    3. Casting — convert between enum ↔ int ↔ string
//    4. Switch on enum — exhaustive, readable branching
//    5. [Flags] enum — combine values with bitwise OR
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    // ── 1. Basic enum ────────────────────────────────────────────────────
    // By default values start at 0 and increment by 1.
    public enum Direction
    {
        North,   // 0
        East,    // 1
        South,   // 2
        West     // 3
    }

    // ── 2. Explicit integer values ────────────────────────────────────────
    // Common for HTTP status codes, database columns, API contracts.
    public enum OrderStatus
    {
        Pending = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 99
    }

    // ── 3. [Flags] enum — combine multiple values ─────────────────────────
    // Values MUST be powers of 2 so bits don't overlap.
    [Flags]
    public enum Permission
    {
        None = 0,
        Read = 1,   // 001
        Write = 2,   // 010
        Delete = 4,   // 100
        All = Read | Write | Delete   // 111
    }

    public static class EnumsDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Enums ===\n");

            // ── Basic usage ──────────────────────────────────────────────
            Direction heading = Direction.North;
            Console.WriteLine($"Heading:          {heading}");          // "North"
            Console.WriteLine($"As integer:       {(int)heading}");     // 0
            Console.WriteLine($"Name as string:   {heading.ToString()}");

            // ── Casting int → enum ───────────────────────────────────────
            int rawValue = 2;
            Direction fromInt = (Direction)rawValue;
            Console.WriteLine($"\nCast {rawValue} → Direction: {fromInt}"); // South

            // ── Parse string → enum (safe with TryParse) ─────────────────
            string input = "West";
            if (Enum.TryParse(input, out Direction parsed))
                Console.WriteLine($"Parsed '{input}' → {parsed}");

            // ── Switch on enum ───────────────────────────────────────────
            Console.WriteLine();
            OrderStatus[] statuses = { OrderStatus.Pending, OrderStatus.Shipped, OrderStatus.Cancelled };
            foreach (var status in statuses)
                Console.WriteLine($"Order {status,10} → {DescribeStatus(status)}");

            // ── Enum.GetValues — iterate all members ─────────────────────
            Console.WriteLine("\nAll directions:");
            foreach (Direction d in Enum.GetValues(typeof(Direction)))
                Console.WriteLine($"  {(int)d} = {d}");

            // ── [Flags] demo ─────────────────────────────────────────────
            Console.WriteLine("\n[Flags] Permission examples:");

            Permission adminPerms = Permission.All;
            Permission guestPerms = Permission.Read;
            Permission editorPerms = Permission.Read | Permission.Write;

            PrintPermissions("Admin", adminPerms);
            PrintPermissions("Guest", guestPerms);
            PrintPermissions("Editor", editorPerms);

            // Check a specific flag
            bool canDelete = editorPerms.HasFlag(Permission.Delete);
            Console.WriteLine($"\nEditor can delete? {canDelete}");  // False
        }

        // ── Helper: switch that handles every case ────────────────────────
        private static string DescribeStatus(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => "Waiting for payment",
                OrderStatus.Processing => "Being packed",
                OrderStatus.Shipped => "On the way!",
                OrderStatus.Delivered => "Enjoy your order 🎉",
                OrderStatus.Cancelled => "Refund issued",
                _ => "Unknown status"
            };
        }

        private static void PrintPermissions(string role, Permission perms)
        {
            Console.WriteLine($"  {role,-8} → {perms}");
        }
    }
}
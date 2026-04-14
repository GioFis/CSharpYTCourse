// ============================================================
//  PropertiesDemo.cs  —  EP.10  Properties
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  A property looks like a field from the outside but runs
//  code (a getter/setter) under the hood — giving you
//  encapsulation without sacrificing readable syntax.
//
//  Key ideas:
//    1. Auto-property          — shortest form, compiler generates backing field
//    2. Full property          — custom get/set with validation logic
//    3. Computed property      — get-only, derived from other fields
//    4. Expression-bodied      — => shorthand for simple get
//    5. init-only setter       — settable at construction only (C# 9+)
//    6. Private setter         — readable everywhere, writable only inside class
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    // ── Class that demonstrates ALL property styles ───────────────────────
    public class BankAccount
    {
        // 1. AUTO-PROPERTY — compiler creates a hidden backing field for you
        //    Use when no validation or logic is needed.
        public string Owner { get; set; }

        // 2. FULL PROPERTY — you write the backing field + get/set manually.
        //    Lets you add validation, logging, notifications, etc.
        private decimal _balance;
        public decimal Balance
        {
            get => _balance;                    // expression-bodied getter (point 4)
            private set                         // point 6: only methods inside this class can set
            {
                if (value < 0)
                    throw new ArgumentException("Balance cannot be negative.");
                _balance = value;
            }
        }

        // 3. COMPUTED PROPERTY — no backing field; derived on every read.
        //    Never stored — always fresh.
        public string Summary => $"{Owner}: {Balance:C}";   // point 4 again

        // 5. INIT-ONLY — can be set in an object initializer, never after.
        public string AccountNumber { get; init; }

        // Auto-property with a default value (C# 6+)
        public bool IsActive { get; set; } = true;

        // ── Constructor ──────────────────────────────────────────────────
        public BankAccount(string owner, string accountNumber, decimal initialBalance)
        {
            Owner = owner;
            AccountNumber = accountNumber;
            Balance = initialBalance;   // calls the private setter
        }

        // ── Business methods that use the private setter ──────────────────
        public void Deposit(decimal amount)
        {
            if (amount <= 0) throw new ArgumentException("Deposit must be positive.");
            Balance += amount;
            Console.WriteLine($"  Deposited {amount:C} → new balance {Balance:C}");
        }

        public void Withdraw(decimal amount)
        {
            if (amount > Balance) throw new InvalidOperationException("Insufficient funds.");
            Balance -= amount;
            Console.WriteLine($"  Withdrew {amount:C} → new balance {Balance:C}");
        }
    }

    // ── Simple read-only DTO using init-only properties ───────────────────
    public class Coordinates
    {
        public double Lat { get; init; }
        public double Lng { get; init; }

        // Computed — distance from origin
        public double DistanceFromOrigin =>
            Math.Sqrt(Lat * Lat + Lng * Lng);

        public override string ToString() =>
            $"({Lat:F4}, {Lng:F4}) — dist from origin: {DistanceFromOrigin:F4}";
    }

    public static class PropertiesDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Properties ===\n");

            // ── BankAccount ───────────────────────────────────────────────
            var account = new BankAccount("Alice", "IT60-1234", 100m);
            Console.WriteLine($"Account created: {account.Summary}");
            Console.WriteLine($"Active: {account.IsActive}");

            account.Deposit(50m);
            account.Withdraw(30m);
            Console.WriteLine($"Final: {account.Summary}");

            // account.Balance = 999; // ← compile error: setter is private ✓

            // ── Validation: setter rejects bad values ─────────────────────
            Console.WriteLine("\n--- Validation in setter ---");
            try
            {
                account.Withdraw(99999m);  // triggers exception
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Caught: {ex.Message}");
            }

            // ── init-only property ────────────────────────────────────────
            Console.WriteLine("\n--- init-only (Coordinates) ---");
            var loc = new Coordinates { Lat = 45.4654, Lng = 9.1859 };
            Console.WriteLine(loc);
            // loc.Lat = 0;  // ← compile error: init-only ✓

            // ── Quick property comparison table ──────────────────────────
            Console.WriteLine("\n--- Property style cheatsheet ---");
            Console.WriteLine("  { get; set; }          Auto: read + write anywhere");
            Console.WriteLine("  { get; private set; }  Auto: read anywhere, write inside class");
            Console.WriteLine("  { get; init; }         Set once at construction, then read-only");
            Console.WriteLine("  { get; }               Read-only auto (set in constructor only)");
            Console.WriteLine("  => expression          Computed, no backing field");
            Console.WriteLine("  private T _f; get/set  Full property with custom logic");
        }
    }
}
// ============================================================
//  NullReference.cs  —  EP.8  Fixing Null Reference in C#
//  Covers:
//    1. What null is and why NullReferenceException happens
//    2. Nullable value types  (int?, bool?, DateTime?)
//    3. Null checking patterns (== null, is null, is not null)
//    4. Null-conditional operator   ?.  and  ?[]
//    5. Null-coalescing operator    ??
//    6. Null-coalescing assignment  ??=
//    7. Combining ?. and ??
//    8. Safe event/delegate invocation with ?.
//    9. Defensive programming — guard clauses
//   10. Nullable reference types (C# 8+ compiler feature, #nullable)
//
//  Based on: Code with Mosh — C# Basics for Beginners series (EP.8)
//
//  KEY IDEA:
//    null means "no object" — a reference that points to nothing.
//    Calling any member on null → NullReferenceException (crash).
//    C# gives us a toolbox to handle null safely WITHOUT crashing.
// ============================================================

namespace ConsoleApp1YTRecap.Basics
{
    // ── Helper classes used throughout the demos ──────────────
    // A simple class to simulate a real-world object that might be null.
    class Customer
    {
        public string Name { get; set; }
        public Address Address { get; set; }   // can be null — not every customer has one

        public Customer(string name, Address address = null)
        {
            Name = name;
            Address = address;
        }
    }

    class Address
    {
        public string City { get; set; }
        public string PostCode { get; set; }

        public Address(string city, string postCode)
        {
            City = city;
            PostCode = postCode;
        }
    }

    // A class with an event — shows safe invocation with ?.
    class OrderProcessor
    {
        // An event that other code can subscribe to
        public event EventHandler OrderCompleted;

        public void Process(string orderName)
        {
            Console.WriteLine($"  Processing order: {orderName}");

            // SAFE way to raise an event — no crash if nobody subscribed
            // OrderCompleted?.Invoke(this, EventArgs.Empty);
            // is equivalent to:
            // if (OrderCompleted != null) OrderCompleted(this, EventArgs.Empty);
            OrderCompleted?.Invoke(this, EventArgs.Empty);
        }
    }

    // ── Main demo class ───────────────────────────────────────
    class NullReference
    {
        public static void Run()
        {
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║     EP.8 — Fixing Null Reference in C#   ║");
            Console.WriteLine("╚══════════════════════════════════════════╝\n");

            DemoWhatIsNull();
            DemoNullableValueTypes();
            DemoNullChecking();
            DemoNullConditionalOperator();
            DemoNullCoalescingOperator();
            DemoNullCoalescingAssignment();
            DemoCombiningOperators();
            DemoSafeEventInvocation();
            DemoGuardClauses();
            DemoNullableReferenceTypes();
        }

        // ────────────────────────────────────────────────────
        //  1. WHAT IS NULL — AND WHY DOES IT CRASH?
        //
        //  Reference types (classes, strings, arrays) can hold
        //  either a reference to an object in memory, OR null.
        //  null = "I point to nothing".
        //
        //  Value types (int, bool, double, struct) CANNOT be null
        //  by default — they always hold a real value.
        //
        //  The crash: NullReferenceException
        //  → you asked C# to follow a pointer that leads nowhere.
        // ────────────────────────────────────────────────────
        static void DemoWhatIsNull()
        {
            Console.WriteLine("─── 1. WHAT IS NULL ─────────────────────\n");

            // String is a reference type — it can be null
            string name = null;   // name points to nothing

            Console.WriteLine("  name is null: " + (name == null));

            // The following line would CRASH with NullReferenceException:
            //   Console.WriteLine(name.Length);   // ← DON'T do this without checking!
            // We wrap it in try/catch here just to SHOW what the error looks like.
            try
            {
                int len = name.Length;   // BOOM — name is null
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"  💥 NullReferenceException caught: {ex.Message}");
                Console.WriteLine("     This is what crashes real apps if not handled.\n");
            }

            // A Customer with no Address — perfectly valid object,
            // but Address property is null inside it.
            var customer = new Customer("Alice");
            Console.WriteLine($"  Customer name: {customer.Name}");
            Console.WriteLine($"  Customer.Address is null: {customer.Address == null}");

            // The following would crash:
            //   Console.WriteLine(customer.Address.City);   // crash!
            Console.WriteLine();
        }

        // ────────────────────────────────────────────────────
        //  2. NULLABLE VALUE TYPES  (T?)
        //
        //  Value types (int, bool, double…) can't be null normally.
        //  Adding ? makes them nullable:  int?  bool?  DateTime?
        //
        //  Under the hood: Nullable<T> — a struct with two fields:
        //    .HasValue  → true if a value is stored
        //    .Value     → the actual value (throws if HasValue is false)
        //    .GetValueOrDefault()  → value or default(T) safely
        //
        //  When to use: database columns, optional parameters,
        //  any scenario where "no value" is a valid state.
        // ────────────────────────────────────────────────────
        static void DemoNullableValueTypes()
        {
            Console.WriteLine("─── 2. NULLABLE VALUE TYPES (T?) ────────\n");

            // Regular int — cannot be null
            int age = 25;

            // Nullable int — CAN be null
            int? optionalAge = null;   // int? is shorthand for Nullable<int>

            Console.WriteLine($"  Regular int age: {age}");
            Console.WriteLine($"  Nullable int? optionalAge: {optionalAge}");
            Console.WriteLine($"  optionalAge.HasValue: {optionalAge.HasValue}");

            // GetValueOrDefault — safe, never throws
            Console.WriteLine($"  optionalAge.GetValueOrDefault(): {optionalAge.GetValueOrDefault()}");
            Console.WriteLine($"  optionalAge.GetValueOrDefault(18): {optionalAge.GetValueOrDefault(18)}");

            // Assign a real value
            optionalAge = 30;
            Console.WriteLine($"\n  After assigning 30:");
            Console.WriteLine($"  optionalAge.HasValue: {optionalAge.HasValue}");
            Console.WriteLine($"  optionalAge.Value:    {optionalAge.Value}");

            // Nullable DateTime — common in databases (e.g., "date of birth not provided")
            DateTime? birthDate = null;
            Console.WriteLine($"\n  birthDate is null: {birthDate == null}");
            birthDate = new DateTime(1995, 6, 15);
            Console.WriteLine($"  birthDate after assignment: {birthDate?.ToString("yyyy-MM-dd")}");

            // Null-conditional on a value type nullable:
            // birthDate?.ToString() returns null if birthDate is null, string otherwise
            Console.WriteLine();
        }

        // ────────────────────────────────────────────────────
        //  3. NULL CHECKING PATTERNS
        //
        //  Several ways to check for null — pick the one that reads best:
        //    a) Classic equality:     if (x == null)
        //    b) Pattern matching:     if (x is null)      (C# 7+)
        //    c) Negative pattern:     if (x is not null)  (C# 9+)
        //    d) Object.ReferenceEquals(x, null) — rarely needed
        //
        //  Mosh's recommendation: 'is null' and 'is not null' are
        //  unambiguous and can't be accidentally overloaded.
        // ────────────────────────────────────────────────────
        static void DemoNullChecking()
        {
            Console.WriteLine("─── 3. NULL CHECKING PATTERNS ───────────\n");

            string input = null;

            // a) Classic equality check
            if (input == null)
                Console.WriteLine("  a) input == null  → classic style, works everywhere");

            // b) Pattern matching with 'is null' (C# 7+)
            //    Slightly safer: can't be tricked by a custom == operator
            if (input is null)
                Console.WriteLine("  b) input is null  → pattern matching style (recommended)");

            // c) Negative pattern 'is not null' (C# 9+)
            string validInput = "hello";
            if (validInput is not null)
                Console.WriteLine($"  c) validInput is not null → value is \"{validInput}\"");

            // d) String-specific: IsNullOrEmpty and IsNullOrWhiteSpace
            //    These go further than just null — they also catch "" and "   "
            string empty = "";
            string whitespace = "   ";

            Console.WriteLine($"\n  string.IsNullOrEmpty(null):       {string.IsNullOrEmpty(null)}");
            Console.WriteLine($"  string.IsNullOrEmpty(\"\"):          {string.IsNullOrEmpty(empty)}");
            Console.WriteLine($"  string.IsNullOrEmpty(\"hello\"):     {string.IsNullOrEmpty("hello")}");
            Console.WriteLine($"  string.IsNullOrWhiteSpace(\"   \"):  {string.IsNullOrWhiteSpace(whitespace)}");

            Console.WriteLine();
        }

        // ────────────────────────────────────────────────────
        //  4. NULL-CONDITIONAL OPERATOR  ?.  and  ?[]
        //
        //  Syntax:  object?.Member
        //  Meaning: "if object is not null, access Member;
        //            otherwise return null (don't crash)."
        //
        //  This short-circuits the entire chain:
        //    a?.b?.c?.d
        //  If 'a' is null → whole expression is null, no crash.
        //  If 'a' is fine but 'b' is null → stops there, returns null.
        //
        //  ?[]  works the same way for array/list indexing.
        //
        //  IMPORTANT: the result type is always nullable.
        //  customer?.Address.City returns string? (possibly null).
        // ────────────────────────────────────────────────────
        static void DemoNullConditionalOperator()
        {
            Console.WriteLine("─── 4. NULL-CONDITIONAL OPERATOR (?.) ───\n");

            // ── Without ?.  (verbose, old style) ─────────────
            Customer c1 = null;

            string city1;
            if (c1 != null && c1.Address != null)
                city1 = c1.Address.City;
            else
                city1 = null;

            Console.WriteLine($"  Old-style null check → city1 = {city1 ?? "(null)"}");

            // ── With ?.  (modern, concise) ─────────────────────
            // If c1 is null → c1?.Address is null → c1?.Address?.City is null. No crash.
            string city2 = c1?.Address?.City;
            Console.WriteLine($"  With ?. operator    → city2 = {city2 ?? "(null)"}");

            // ── Deep chain example ──────────────────────────────
            Customer c2 = new Customer("Bob", new Address("Milan", "20100"));
            string postCode = c2?.Address?.PostCode;
            Console.WriteLine($"\n  c2 (with address) → PostCode = {postCode}");

            Customer c3 = new Customer("Charlie");   // Address is null
            string postCode2 = c3?.Address?.PostCode;
            Console.WriteLine($"  c3 (no address)   → PostCode = {postCode2 ?? "(null)"}");

            // ── ?[] for arrays/lists ──────────────────────────
            int[] numbers = null;
            int? firstElement = numbers?[0];   // if numbers is null → null, no IndexOutOfRange
            Console.WriteLine($"\n  numbers?[0] when numbers is null: {firstElement?.ToString() ?? "(null)"}");

            numbers = new int[] { 10, 20, 30 };
            firstElement = numbers?[0];
            Console.WriteLine($"  numbers?[0] when numbers = [10,20,30]: {firstElement}");

            // ── On methods ───────────────────────────────────
            string msg = null;
            string upper = msg?.ToUpper();   // msg is null → upper is null, no crash
            Console.WriteLine($"\n  null?.ToUpper() = {upper ?? "(null)"}");

            msg = "hello";
            upper = msg?.ToUpper();
            Console.WriteLine($"  \"hello\"?.ToUpper() = {upper}");

            Console.WriteLine();
        }

        // ────────────────────────────────────────────────────
        //  5. NULL-COALESCING OPERATOR  ??
        //
        //  Syntax:  expression ?? fallback
        //  Meaning: "if expression is not null, use it;
        //            otherwise use fallback."
        //
        //  Think of it as a one-liner for:
        //    var result = expression != null ? expression : fallback;
        //
        //  The fallback (right side) is ONLY evaluated if the left
        //  side is null — this is called "lazy evaluation".
        //
        //  Can be chained:  a ?? b ?? c ?? "default"
        // ────────────────────────────────────────────────────
        static void DemoNullCoalescingOperator()
        {
            Console.WriteLine("─── 5. NULL-COALESCING OPERATOR (??) ────\n");

            string name = null;

            // Old style:
            string displayName1 = (name != null) ? name : "Guest";
            Console.WriteLine($"  Old style ternary: {displayName1}");

            // With ?? — much cleaner:
            string displayName2 = name ?? "Guest";
            Console.WriteLine($"  With ??           : {displayName2}");

            // When name has a value:
            name = "Alice";
            string displayName3 = name ?? "Guest";
            Console.WriteLine($"  name = \"Alice\"    : {displayName3}");

            // With nullable int:
            int? score = null;
            int finalScore = score ?? 0;   // if no score, default to 0
            Console.WriteLine($"\n  score ?? 0 when score is null: {finalScore}");

            score = 85;
            finalScore = score ?? 0;
            Console.WriteLine($"  score ?? 0 when score = 85:    {finalScore}");

            // Chaining ?? — first non-null wins
            string a = null;
            string b = null;
            string c = "Found me!";
            string result = a ?? b ?? c ?? "ultimate fallback";
            Console.WriteLine($"\n  a ?? b ?? c ?? \"ultimate fallback\": {result}");

            Console.WriteLine();
        }

        // ────────────────────────────────────────────────────
        //  6. NULL-COALESCING ASSIGNMENT  ??=   (C# 8+)
        //
        //  Syntax:  variable ??= value
        //  Meaning: "assign value to variable ONLY if variable is null."
        //
        //  Equivalent to:
        //    if (variable == null) variable = value;
        //
        //  Very useful for lazy initialization — initialize something
        //  only the first time it's needed.
        // ────────────────────────────────────────────────────
        static void DemoNullCoalescingAssignment()
        {
            Console.WriteLine("─── 6. NULL-COALESCING ASSIGNMENT (??=) ─\n");

            string config = null;

            // Old style:
            // if (config == null) config = "default-config";

            // With ??=:
            config ??= "default-config";
            Console.WriteLine($"  After ??= on null  : config = \"{config}\"");

            // If already has a value — ??= does NOTHING:
            config ??= "another-value";
            Console.WriteLine($"  After ??= on non-null: config = \"{config}\"  (unchanged)");

            // Lazy initialization of a list:
            List<string> tags = null;
            Console.WriteLine($"\n  tags before ??= : {(tags == null ? "null" : "initialized")}");

            (tags ??= new List<string>()).Add("csharp");
            tags.Add("dotnet");
            Console.WriteLine($"  tags after ??=  : [{string.Join(", ", tags)}]");

            Console.WriteLine();
        }

        // ────────────────────────────────────────────────────
        //  7. COMBINING  ?.  AND  ??
        //
        //  The two operators are designed to work together:
        //    object?.Property ?? fallback
        //
        //  If object is null  → ?. returns null → ?? kicks in → fallback
        //  If object is valid → ?. returns value → ?? not needed
        //
        //  This is the pattern Mosh uses most often in real code.
        // ────────────────────────────────────────────────────
        static void DemoCombiningOperators()
        {
            Console.WriteLine("─── 7. COMBINING ?. AND ?? ──────────────\n");

            Customer withAddress = new Customer("Dave", new Address("Rome", "00100"));
            Customer withoutAddress = new Customer("Eve");   // Address = null
            Customer nullCustomer = null;

            // Single expression handles ALL three cases safely:
            //   customer?.Address?.City  →  null if either is null
            //   ?? "City unknown"        →  fallback shown to user
            string Display(Customer cust)
                => cust?.Address?.City ?? "City unknown";

            Console.WriteLine($"  Dave   (has address)  → {Display(withAddress)}");
            Console.WriteLine($"  Eve    (no address)   → {Display(withoutAddress)}");
            Console.WriteLine($"  null customer         → {Display(nullCustomer)}");

            // Combining with method calls:
            Customer c = new Customer("Frank");   // no Address
            int cityLength = c?.Address?.City?.Length ?? -1;
            Console.WriteLine($"\n  city?.Length ?? -1 when no address: {cityLength}");

            // With nullable int?
            int? nullableVal = null;
            Console.WriteLine($"  nullableVal?.ToString() ?? \"N/A\" : {nullableVal?.ToString() ?? "N/A"}");

            Console.WriteLine();
        }

        // ────────────────────────────────────────────────────
        //  8. SAFE EVENT INVOCATION WITH ?.
        //
        //  Events in C# are delegates — they can be null if nobody
        //  has subscribed. Calling Invoke() on null → crash.
        //
        //  Old (unsafe) pattern:
        //    if (OrderCompleted != null)
        //        OrderCompleted(this, EventArgs.Empty);
        //
        //  Modern (safe) pattern with ?.Invoke():
        //    OrderCompleted?.Invoke(this, EventArgs.Empty);
        //
        //  The ?. version is also thread-safe:
        //  it captures the delegate reference before checking null,
        //  preventing a race condition where another thread could
        //  unsubscribe between the null-check and the invocation.
        // ────────────────────────────────────────────────────
        static void DemoSafeEventInvocation()
        {
            Console.WriteLine("─── 8. SAFE EVENT INVOCATION (?.) ───────\n");

            var processor = new OrderProcessor();

            // No subscribers yet — without ?. this would crash
            Console.WriteLine("  Processing with NO subscribers (safe thanks to ?.Invoke):");
            processor.Process("Order-001");   // OrderCompleted?.Invoke → no crash

            // Now subscribe to the event
            processor.OrderCompleted += (sender, args) =>
                Console.WriteLine("  ✅ Subscriber notified: order is complete!");

            Console.WriteLine("\n  Processing WITH a subscriber:");
            processor.Process("Order-002");   // fires the event, subscriber runs

            Console.WriteLine();
        }

        // ────────────────────────────────────────────────────
        //  9. DEFENSIVE PROGRAMMING — GUARD CLAUSES
        //
        //  The best fix for NullReferenceException is to PREVENT
        //  null from entering your methods in the first place.
        //
        //  A "guard clause" is an early return (or throw) at the
        //  top of a method that rejects invalid input immediately,
        //  keeping the rest of the method clean.
        //
        //  ArgumentNullException is the standard exception to
        //  throw when a required parameter is null.
        // ────────────────────────────────────────────────────
        static void DemoGuardClauses()
        {
            Console.WriteLine("─── 9. GUARD CLAUSES ────────────────────\n");

            // Call with a valid customer
            try
            {
                PrintCustomerCity(new Customer("Grace", new Address("Paris", "75001")));
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  Error: {ex.Message}");
            }

            // Call with null customer — guard clause catches it cleanly
            try
            {
                PrintCustomerCity(null);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  Caught ArgumentNullException: {ex.ParamName} cannot be null");
            }

            // Call with customer that has no address — second guard catches it
            try
            {
                PrintCustomerCity(new Customer("Henry"));
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  Caught ArgumentNullException: {ex.ParamName} cannot be null");
            }

            Console.WriteLine();
        }

        // Method with guard clauses — fails fast and clearly
        static void PrintCustomerCity(Customer customer)
        {
            // Guard clause 1: reject null customer immediately
            if (customer is null)
                throw new ArgumentNullException(nameof(customer));

            // Guard clause 2: reject customer without address
            if (customer.Address is null)
                throw new ArgumentNullException(nameof(customer.Address),
                    $"Customer '{customer.Name}' has no address.");

            // If we reach here, both are guaranteed non-null
            Console.WriteLine($"  Customer: {customer.Name}, City: {customer.Address.City}");
        }

        // ────────────────────────────────────────────────────
        // 10. NULLABLE REFERENCE TYPES  (C# 8+ compiler feature)
        //
        //  Enabled with:  #nullable enable   (or in .csproj)
        //
        //  When enabled, the compiler distinguishes:
        //    string    → non-nullable  (compiler warns if you assign null)
        //    string?   → nullable      (compiler warns if you use without null-check)
        //
        //  This shifts null bugs from RUNTIME to COMPILE TIME.
        //
        //  Operators for edge cases:
        //    !  (null-forgiving operator) — tells compiler "trust me, not null here"
        //       e.g.  someString!.Length  — suppresses the warning
        //       Use sparingly — it turns off the safety net.
        //
        //  NOTE: this is a COMPILER FEATURE only, no runtime change.
        //  The ! operator has zero effect at runtime.
        // ────────────────────────────────────────────────────
        static void DemoNullableReferenceTypes()
        {
            Console.WriteLine("─── 10. NULLABLE REFERENCE TYPES ────────\n");

            Console.WriteLine("  With #nullable enable in the project (or .csproj):");
            Console.WriteLine("    string  name;    → compiler warns if assigned null");
            Console.WriteLine("    string? name;    → explicitly says 'this can be null'");
            Console.WriteLine("    name!.Length     → null-forgiving: suppress warning (use sparingly)\n");

            // Demonstrating the intent — these are valid in any mode,
            // but with #nullable enable the compiler would warn if you
            // pass a non-nullable 'string' where you meant 'string?'.

            string nonNullable = "I must have a value";
            string? nullable = null;

            Console.WriteLine($"  nonNullable: \"{nonNullable}\"");
            Console.WriteLine($"  nullable:    {(nullable is null ? "(null)" : nullable)}");

            // Safe usage: always check before using a nullable reference
            if (nullable is not null)
                Console.WriteLine($"  nullable.ToUpper(): {nullable.ToUpper()}");
            else
                Console.WriteLine("  nullable is null — skipping .ToUpper() safely");

            Console.WriteLine();
            Console.WriteLine("═══════════════════════════════════════════");
            Console.WriteLine("  SUMMARY — Your null-safety toolkit:");
            Console.WriteLine("  T?      → make a value type nullable (int?, bool?)");
            Console.WriteLine("  == null / is null  → check if something is null");
            Console.WriteLine("  ?.      → access member only if not null (safe navigation)");
            Console.WriteLine("  ?[]     → index into collection only if not null");
            Console.WriteLine("  ??      → provide a fallback value when null");
            Console.WriteLine("  ??=     → assign only if currently null (lazy init)");
            Console.WriteLine("  ?.Invoke→ raise event safely (thread-safe null check)");
            Console.WriteLine("  Guard   → throw ArgumentNullException early, fail fast");
            Console.WriteLine("  string? → C# 8 nullable refs: move bugs to compile time");
            Console.WriteLine("═══════════════════════════════════════════\n");
        }
    }
}
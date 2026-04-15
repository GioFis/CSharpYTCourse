// ============================================================
//  IsAsCastingDemo.cs  —  EP.10  is / as / casting
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  When you store objects through a base type or interface,
//  you sometimes need to recover the CONCRETE type.
//  C# gives you three tools for this:
//
//  (TargetType) obj   — EXPLICIT CAST
//    Forces the conversion. Throws InvalidCastException if wrong type.
//    Use when you are 100% certain of the type.
//
//  obj is TargetType  — TYPE CHECK
//    Returns true/false. Safe — never throws.
//    C# 7+: `obj is TargetType x` — checks AND declares variable in one step.
//
//  obj as TargetType  — SAFE CAST
//    Returns the cast object OR null. Never throws.
//    Only works with reference types and nullable value types.
//    Always check for null before using the result.
//
//  Order of preference (safest → fastest):
//    is pattern  >  as + null check  >  explicit cast (when certain)
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    // ── Type hierarchy used across all sections ───────────────────────────
    public abstract class Vehicle
    {
        public string Brand { get; set; }
        public int Year { get; set; }
        protected Vehicle(string brand, int year) { Brand = brand; Year = year; }
        public virtual string Describe() => $"{Year} {Brand}";
    }

    public class Car : Vehicle
    {
        public int Doors { get; set; }
        public Car(string brand, int year, int doors = 4)
            : base(brand, year) => Doors = doors;
        public override string Describe() => $"{base.Describe()} (Car, {Doors}d)";
        public void Honk() => Console.WriteLine($"  {Brand}: BEEP BEEP!");
    }

    public class Truck : Vehicle
    {
        public double PayloadTons { get; set; }
        public Truck(string brand, int year, double payload)
            : base(brand, year) => PayloadTons = payload;
        public override string Describe() => $"{base.Describe()} (Truck, {PayloadTons}t)";
        public void LoadCargo(string item) =>
            Console.WriteLine($"  {Brand} loads: {item}");
    }

    public class ElectricCar : Car
    {
        public int RangeKm { get; set; }
        public ElectricCar(string brand, int year, int range)
            : base(brand, year) => RangeKm = range;
        public override string Describe() => $"{base.Describe()} ⚡ range={RangeKm}km";
        public void Charge() => Console.WriteLine($"  {Brand} charging...");
    }

    public static class IsAsCastingDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 is / as / casting ===\n");

            // Build a mixed list — all stored as Vehicle (base type)
            var fleet = new List<Vehicle>
            {
                new Car("Toyota", 2020),
                new Truck("Volvo", 2019, payload: 10.5),
                new ElectricCar("Tesla", 2023, range: 550),
                new Car("Ford", 2021, doors: 2),
                new Truck("Scania", 2022, payload: 20),
                new ElectricCar("Rivian", 2024, range: 480),
            };

            ExplicitCastDemo(fleet);
            IsCheckDemo(fleet);
            IsPatternDemo(fleet);
            AsDemo(fleet);
            NullCheckPattern(fleet);
            InheritanceChainDemo(fleet);
            SummaryTable();
        }

        // ── 1. Explicit cast — (TargetType) obj ──────────────────────────
        // Throws InvalidCastException if the object is not that type.
        private static void ExplicitCastDemo(List<Vehicle> fleet)
        {
            Console.WriteLine("--- 1. Explicit Cast (TargetType) obj ---");
            Console.WriteLine("  Throws InvalidCastException on wrong type.\n");

            Vehicle v = fleet[0];    // Toyota Car

            // We KNOW v is a Car — safe to cast
            Car car = (Car)v;
            Console.WriteLine($"  Cast succeeded: {car.Describe()}");
            car.Honk();

            // Wrong cast — will throw
            Console.WriteLine("\n  Trying wrong cast (Car → Truck)...");
            try
            {
                Truck truck = (Truck)fleet[0];   // Toyota is NOT a Truck
                Console.WriteLine($"  {truck.Describe()}");
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"  ✗ InvalidCastException: {ex.Message}");
            }
        }

        // ── 2. is — type check only ───────────────────────────────────────
        // Returns true/false. Safe — never throws. Does NOT give you the cast object.
        private static void IsCheckDemo(List<Vehicle> fleet)
        {
            Console.WriteLine("\n--- 2. is keyword — type check ---");
            Console.WriteLine("  Returns bool. Safe, no exception.\n");

            foreach (Vehicle v in fleet)
            {
                bool isCar = v is Car;
                bool isTruck = v is Truck;
                bool isElectric = v is ElectricCar;
                Console.WriteLine($"  {v.Brand,-10} | Car={isCar,-5} Truck={isTruck,-5} Electric={isElectric}");
            }

            // is also works for null check
            Vehicle? maybeNull = null;
            Console.WriteLine($"\n  null is Vehicle: {maybeNull is Vehicle}");   // false
            Console.WriteLine($"  null is null:    {maybeNull is null}");        // true
        }

        // ── 3. is PATTERN — check + declare in one step (C# 7+) ──────────
        // This is the PREFERRED modern approach.
        // If the check passes, the variable is immediately usable.
        private static void IsPatternDemo(List<Vehicle> fleet)
        {
            Console.WriteLine("\n--- 3. is Pattern Matching (C# 7+) — check + cast in one step ---");
            Console.WriteLine("  Preferred modern style: `if (obj is Car c)` \n");

            foreach (Vehicle v in fleet)
            {
                if (v is ElectricCar ev)    // check AND declare 'ev' in one line
                {
                    Console.WriteLine($"  ⚡ {ev.Describe()}");
                    ev.Charge();
                }
                else if (v is Truck t)
                {
                    Console.WriteLine($"  🚚 {t.Describe()}");
                    t.LoadCargo("Furniture");
                }
                else if (v is Car c)        // Car AFTER ElectricCar (which IS also a Car)
                {
                    Console.WriteLine($"  🚗 {c.Describe()}");
                    c.Honk();
                }
            }
        }

        // ── 4. as — safe cast, returns null on failure ────────────────────
        // Never throws. Returns null if the object is the wrong type.
        // Only works with reference types.
        private static void AsDemo(List<Vehicle> fleet)
        {
            Console.WriteLine("\n--- 4. as keyword — safe cast, null on failure ---");
            Console.WriteLine("  Returns null instead of throwing. Always null-check the result.\n");

            foreach (Vehicle v in fleet)
            {
                Car? car = v as Car;   // null if v is not a Car (or subtype)

                if (car != null)
                    Console.WriteLine($"  as Car → OK  : {car.Describe()}");
                else
                    Console.WriteLine($"  as Car → null: {v.Describe()} is not a Car");
            }
        }

        // ── 5. The null-check pattern after 'as' ─────────────────────────
        private static void NullCheckPattern(List<Vehicle> fleet)
        {
            Console.WriteLine("\n--- 5. Null-check pattern after 'as' ---");
            Console.WriteLine("  Count trucks and total their payload:\n");

            double totalPayload = 0;
            int truckCount = 0;

            foreach (Vehicle v in fleet)
            {
                Truck? t = v as Truck;
                if (t == null) continue;          // not a truck — skip

                totalPayload += t.PayloadTons;
                truckCount++;
            }

            Console.WriteLine($"  Trucks in fleet: {truckCount}");
            Console.WriteLine($"  Total payload:   {totalPayload:F1} tons");
        }

        // ── 6. Inheritance chain — is respects the full chain ─────────────
        // ElectricCar IS a Car IS a Vehicle
        private static void InheritanceChainDemo(List<Vehicle> fleet)
        {
            Console.WriteLine("\n--- 6. is respects the full inheritance chain ---");

            var tesla = fleet.OfType<ElectricCar>().First();

            Console.WriteLine($"  tesla is ElectricCar : {tesla is ElectricCar}");  // true
            Console.WriteLine($"  tesla is Car         : {tesla is Car}");          // true!
            Console.WriteLine($"  tesla is Vehicle     : {tesla is Vehicle}");      // true!
            Console.WriteLine($"  tesla is Truck       : {tesla is Truck}");        // false

            // Useful: OfType<T>() on LINQ — filters and casts in one step
            Console.WriteLine("\n  LINQ OfType<ElectricCar>() — filter to only electric:");
            foreach (var ev in fleet.OfType<ElectricCar>())
                Console.WriteLine($"    {ev.Describe()}");
        }

        private static void SummaryTable()
        {
            Console.WriteLine("\n--- Summary: which tool to use? ---\n");
            Console.WriteLine($"  {"Tool",-25} {"Throws?",-10} {"Returns",-20} {"Use when"}");
            Console.WriteLine("  " + new string('-', 80));

            var rows = new[]
            {
                ("(T) obj",           "Yes",  "T",         "Certain of type; fail = bug"),
                ("obj is T",          "No",   "bool",      "Just need to know the type"),
                ("obj is T x",        "No",   "bool + T x","Check AND use — prefer this"),
                ("obj as T",          "No",   "T or null", "Unsure; always null-check"),
                ("LINQ OfType<T>()",  "No",   "IEnumerable<T>", "Filter a collection by type"),
            };

            foreach (var (tool, throws, returns, when) in rows)
                Console.WriteLine($"  {tool,-25} {throws,-10} {returns,-20} {when}");
        }
    }
}
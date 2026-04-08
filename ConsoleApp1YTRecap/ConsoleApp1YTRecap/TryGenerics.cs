// ============================================================
//  TryGenerics.cs  —  EP.7 DEMO RUNNER
//
//  Mirrors Code Monkey's teaching flow:
//   1. The problem — duplicate classes without generics
//   2. The solution — Inventory<T>, one class for everything
//   3. Type safety  — wrong type = compile-time error (explained in text)
//   4. Generic methods — Swap<T>, Print<T>
//   5. Constrained generics — where T : IAttackable
//   6. Built-in generics  — List<T>, Dictionary<TKey,TValue>
// ============================================================

namespace TryPlay1
{
    public static class TryGenerics
    {
        public static void Run()
        {
            Console.WriteLine("══════════════════════════════════════════════");
            Console.WriteLine("  EP.7 — GENERICS");
            Console.WriteLine("══════════════════════════════════════════════\n");

            // ── SECTION 1: The Problem ────────────────────────────────────────
            Console.WriteLine("── SECTION 1: The problem generics solve ──\n");

            Console.WriteLine("  Without generics you'd write a separate Inventory class");
            Console.WriteLine("  for every type you want to store:\n");
            Console.WriteLine("    class WeaponInventory  { Weapon[]  _slots; ... }");
            Console.WriteLine("    class PotionInventory  { Potion[]  _slots; ... }");
            Console.WriteLine("    class GoldInventory    { GoldCoin[] _slots; ... }");
            Console.WriteLine();
            Console.WriteLine("  The logic (Add, Get, Count) is IDENTICAL in all three.");
            Console.WriteLine("  Copy-pasting identical code is a maintenance nightmare.");
            Console.WriteLine("  Generics let you write it ONCE with a type placeholder <T>.\n");

            PressEnter();

            // ── SECTION 2: Generic class — Inventory<T> ───────────────────────
            Console.WriteLine("── SECTION 2: Inventory<T> — one class, any type ──\n");

            Console.WriteLine("  Syntax:  public class Inventory<T>  { ... }");
            Console.WriteLine("  T is replaced by the real type when you create an instance:\n");

            // Weapon inventory — T becomes Weapon
            Console.WriteLine("  → new Inventory<Weapon>(3)  — T is now Weapon everywhere");
            var weaponBag = new Inventory<Weapon>(3);
            weaponBag.Add(new Weapon("Excalibur", 80));
            weaponBag.Add(new Weapon("Iron Sword", 30));
            weaponBag.Add(new Weapon("Dagger", 15));
            weaponBag.Add(new Weapon("Hidden Blade", 20)); // over capacity → warning shown
            weaponBag.PrintAll("Weapon Bag");

            Console.WriteLine();

            // Potion inventory — exactly the same class, T is now Potion
            Console.WriteLine("  → new Inventory<Potion>(4)  — T is now Potion everywhere");
            var potionBag = new Inventory<Potion>(4);
            potionBag.Add(new Potion("Health Potion", 50));
            potionBag.Add(new Potion("Mana Potion", 40));
            potionBag.Add(new Potion("Elixir", 100));
            potionBag.PrintAll("Potion Bag");

            Console.WriteLine();

            // Gold inventory — same class again, T is GoldCoin
            Console.WriteLine("  → new Inventory<GoldCoin>(5)  — T is now GoldCoin");
            var goldBag = new Inventory<GoldCoin>(5);
            goldBag.Add(new GoldCoin(100));
            goldBag.Add(new GoldCoin(50));
            goldBag.PrintAll("Gold Bag");

            PressEnter();

            // ── SECTION 3: Type safety ────────────────────────────────────────
            Console.WriteLine("── SECTION 3: Type safety — the compile-time guarantee ──\n");

            Console.WriteLine("  This is one of generics' biggest advantages over using 'object'.");
            Console.WriteLine("  If you tried to write:  weaponBag.Add(new Potion(...))");
            Console.WriteLine("  the compiler refuses immediately — you never even run the code.");
            Console.WriteLine();
            Console.WriteLine("  Compare that to the old non-generic ArrayList:");
            Console.WriteLine("    ArrayList list = new ArrayList();");
            Console.WriteLine("    list.Add(new Weapon(...));");
            Console.WriteLine("    list.Add(new Potion(...));   ← compiles fine! bug hidden.");
            Console.WriteLine("    Weapon w = (Weapon)list[1]; ← crashes at RUNTIME.");
            Console.WriteLine();
            Console.WriteLine("  With Inventory<Weapon> the mistake is caught at compile time,");
            Console.WriteLine("  before the program ever runs. That's type-safe code.\n");

            // Demonstrate Get() — return type is already Weapon, no cast needed.
            Console.WriteLine("  Get() also returns T directly — no casting required:");
            Weapon retrieved = weaponBag.Get(0); // returns Weapon, not object
            Console.WriteLine($"  Retrieved from slot 0: {retrieved}\n");

            PressEnter();

            // ── SECTION 4: Generic methods ────────────────────────────────────
            Console.WriteLine("── SECTION 4: Generic methods — Swap<T> and Print<T> ──\n");

            Console.WriteLine("  Generic methods have their own <T> right after the method name.");
            Console.WriteLine("  Syntax:  public static void Swap<T>(ref T a, ref T b)");
            Console.WriteLine("  The compiler INFERS T from what you pass in — no need to");
            Console.WriteLine("  write Swap<Weapon>(...) explicitly (though you can).\n");

            // Swap two weapons — T inferred as Weapon
            var sword = new Weapon("Sword", 40);
            var axe = new Weapon("Battle Axe", 60);
            Console.WriteLine("  Before swap:");
            InventoryUtils.Print("  slot A", sword);
            InventoryUtils.Print("  slot B", axe);

            InventoryUtils.Swap(ref sword, ref axe); // T inferred = Weapon

            Console.WriteLine("  After swap:");
            InventoryUtils.Print("  slot A", sword);
            InventoryUtils.Print("  slot B", axe);

            Console.WriteLine();

            // Same Swap method, now with Potion — T inferred as Potion
            var hp = new Potion("Health Potion", 50);
            var mp = new Potion("Mana Potion", 40);
            Console.WriteLine("  Same Swap method, different type (Potion — T inferred):");
            InventoryUtils.Print("  before — slot A", hp);
            InventoryUtils.Print("  before — slot B", mp);
            InventoryUtils.Swap(ref hp, ref mp);
            InventoryUtils.Print("  after  — slot A", hp);
            InventoryUtils.Print("  after  — slot B", mp);

            PressEnter();

            // ── SECTION 5: Constrained generics — where T : IAttackable ──────
            Console.WriteLine("── SECTION 5: Constraints — where T : IAttackable ──\n");

            Console.WriteLine("  Sometimes T needs to be CAPABLE of something specific.");
            Console.WriteLine("  You add a constraint:  class BattleSlot<T> where T : IAttackable");
            Console.WriteLine("  Now the compiler knows T has .Health and .TakeDamage().");
            Console.WriteLine("  You can call those methods on T without any casting.\n");

            // BattleSlot<Hero> — T = Hero, which implements IAttackable ✓
            Console.WriteLine("  BattleSlot<Hero> — valid because Hero : IAttackable");
            var heroSlot = new BattleSlot<Hero>(new Hero("Lancelot", 60));
            heroSlot.ApplyDot(damagePerTick: 10, ticks: 4);

            Console.WriteLine();

            // BattleSlot<Monster> — same slot class, different T
            Console.WriteLine("  BattleSlot<Monster> — same class, T = Monster");
            var monsterSlot = new BattleSlot<Monster>(new Monster("Orc", 45));
            monsterSlot.ApplyDot(damagePerTick: 15, ticks: 2);

            Console.WriteLine();
            Console.WriteLine("  If you tried  new BattleSlot<Potion>(...)  the compiler");
            Console.WriteLine("  would refuse — Potion doesn't implement IAttackable.\n");

            PressEnter();

            // ── SECTION 6: Built-in generics (Code Monkey always shows these) ─
            Console.WriteLine("── SECTION 6: Built-in generics you already use ──\n");

            Console.WriteLine("  .NET's own collections are all generic — you've been using");
            Console.WriteLine("  generics all along without realising it:\n");

            // List<T> — the most common built-in generic
            Console.WriteLine("  List<string> — a resizable array of strings:");
            var names = new List<string> { "Arthur", "Merlin", "Guinevere" };
            names.Add("Lancelot");
            names.ForEach(n => Console.WriteLine($"    • {n}"));

            Console.WriteLine();

            // Dictionary<TKey, TValue> — two type params
            Console.WriteLine("  Dictionary<string, int> — two type params TKey,TValue:");
            var scores = new Dictionary<string, int>
            {
                { "Arthur",  950 },
                { "Merlin",  870 },
                { "Lancelot",1100 }
            };
            foreach (var kv in scores)
                Console.WriteLine($"    {kv.Key,-12} → {kv.Value} pts");

            Console.WriteLine();

            // default(T) reminder — comes up often with generics
            Console.WriteLine("  default(T) — the 'zero' value for any T:");
            Console.WriteLine($"    default(int)    = {default(int)}");
            Console.WriteLine($"    default(bool)   = {default(bool)}");
            Console.WriteLine($"    default(string) = {(default(string) == null ? "null" : default(string))}");
            Console.WriteLine($"    default(Weapon) = {(default(Weapon) == null ? "null (reference type)" : "")}");

            PressEnter();

            // ── SUMMARY ───────────────────────────────────────────────────────
            Console.WriteLine("── SUMMARY ──\n");
            Console.WriteLine("  Generic class    = class MyClass<T>  { }");
            Console.WriteLine("                     Write once, reuse for any type.");
            Console.WriteLine();
            Console.WriteLine("  Generic method   = void Swap<T>(ref T a, ref T b)");
            Console.WriteLine("                     Compiler infers T from arguments.");
            Console.WriteLine();
            Console.WriteLine("  Constraint       = where T : IAttackable");
            Console.WriteLine("                     Restricts T and unlocks interface members.");
            Console.WriteLine();
            Console.WriteLine("  Type safety      = wrong type → compile-time error, not crash.");
            Console.WriteLine();
            Console.WriteLine("  default(T)       = null for classes, 0 for value types.");
            Console.WriteLine();
            Console.WriteLine("  You already use generics: List<T>, Dictionary<TKey,TValue>.");
            Console.WriteLine("\n══════════════════════════════════════════════\n");
        }

        private static void PressEnter()
        {
            Console.WriteLine("\n  [Press Enter to continue...]");
            Console.ReadLine();
            Console.WriteLine();
        }
    }
}
// ============================================================
//  TryInterface.cs  —  EP.6 DEMO RUNNER
//  Mirrors Code Monkey's teaching flow:
//   1. Why do we need interfaces? (the problem)
//   2. What IS an interface?
//   3. How to declare and implement one
//   4. The real power: polymorphism through a contract
//   5. Bonus: is / as casting
// ============================================================

namespace TryPlay1
{
    public static class TryInterface
    {
        public static void Run()
        {
            Console.WriteLine("══════════════════════════════════════════════");
            Console.WriteLine("  EP.6 — INTERFACES");
            Console.WriteLine("══════════════════════════════════════════════\n");

            // ── SECTION 1: The Problem ────────────────────────────────────────
            Console.WriteLine("── SECTION 1: Why do we need interfaces? ──\n");
            Console.WriteLine("  Imagine a warrior who can attack heroes, monsters, and chests.");
            Console.WriteLine("  Without an interface you'd write three overloads:");
            Console.WriteLine("    Attack(Hero h)         { h.TakeDamage(x); }");
            Console.WriteLine("    Attack(Monster m)      { m.TakeDamage(x); }");
            Console.WriteLine("    Attack(TreasureChest c){ c.TakeDamage(x); }");
            Console.WriteLine("  Add a Dragon tomorrow → add a fourth overload. Not scalable.\n");
            PressEnter();

            // ── SECTION 2: The Contract ───────────────────────────────────────
            Console.WriteLine("── SECTION 2: The interface CONTRACT ──\n");
            Console.WriteLine("  public interface IAttackable");
            Console.WriteLine("  {");
            Console.WriteLine("      int  Health  { get; }");
            Console.WriteLine("      void TakeDamage(int amount);");
            Console.WriteLine("  }");
            Console.WriteLine();
            Console.WriteLine("  Rules Code Monkey drills in:");
            Console.WriteLine("  • Name starts with capital I  (IAttackable, IInteractable...)");
            Console.WriteLine("  • No fields, no constructors, no access modifiers on members");
            Console.WriteLine("  • A class can implement MULTIPLE interfaces (unlike inheritance)");
            Console.WriteLine("  • The interface has NO code — just the promise\n");
            PressEnter();

            // ── SECTION 3: Live demo — Warrior attacks different things ───────
            Console.WriteLine("── SECTION 3: One method, three targets ──\n");

            var warrior = new Warrior("Arthur", attackPower: 25);
            var hero = new Hero("Lancelot", health: 80);
            var goblin = new Monster("Goblin King", health: 60);
            var chest = new TreasureChest("Golden Chest");

            // All three go through the SAME Warrior.Attack(IAttackable) method.
            warrior.Attack(hero);
            warrior.Attack(goblin);
            warrior.Attack(chest);

            Console.WriteLine();
            PressEnter();

            // ── SECTION 4: List of IAttackable (the real power) ───────────────
            Console.WriteLine("── SECTION 4: Polymorphism — list of IAttackable ──\n");
            Console.WriteLine("  Code Monkey's key insight: store everything in List<IAttackable>.");
            Console.WriteLine("  Loop once → hits everything that can be attacked.\n");

            var targets = new List<IAttackable>
            {
                new Hero("Percival", health: 100),
                new Monster("Orc", health: 50),
                new Monster("Troll", health: 70),
                new TreasureChest("Iron Chest"),
                new Hero("Galahad", health: 90),
            };

            var paladin = new Warrior("Paladin", attackPower: 30);

            Console.WriteLine($"  {paladin.Name} sweeps the battlefield:\n");
            foreach (IAttackable t in targets)
                paladin.Attack(t);

            Console.WriteLine();
            PressEnter();

            // ── SECTION 5: is / as — checking the real type ───────────────────
            Console.WriteLine("── SECTION 5: 'is' and 'as' casting ──\n");
            Console.WriteLine("  Sometimes you need to know what something ACTUALLY is.\n");

            foreach (IAttackable t in targets)
            {
                if (t is Hero h)
                    Console.WriteLine($"  → {h.Name} is a Hero (HP remaining: {h.Health})");
                else if (t is Monster m)
                    Console.WriteLine($"  → {m.Name} is a Monster (HP remaining: {m.Health})");
                else if (t is TreasureChest c)
                    Console.WriteLine($"  → [{c.Name}] is a Chest (Durability: {c.Health})");
            }

            Console.WriteLine();
            PressEnter();

            // ── SUMMARY ───────────────────────────────────────────────────────
            Console.WriteLine("── SUMMARY ──\n");
            Console.WriteLine("  Interface  = a CONTRACT (what, not how)");
            Console.WriteLine("  Class      = the IMPLEMENTATION (how)");
            Console.WriteLine("  Benefit 1  = one method works for any implementing type");
            Console.WriteLine("  Benefit 2  = List<IAttackable> groups unrelated types");
            Console.WriteLine("  Benefit 3  = add new types without changing existing code");
            Console.WriteLine("  Benefit 4  = 'is'/'as' lets you check the real type when needed");
            Console.WriteLine("\n══════════════════════════════════════════════\n");
        }

        private static void PressEnter()
        {
            Console.WriteLine("  [Press Enter to continue...]");
            Console.ReadLine();
        }
    }
}
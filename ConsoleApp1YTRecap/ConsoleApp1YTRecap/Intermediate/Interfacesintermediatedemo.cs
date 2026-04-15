// ============================================================
//  InterfacesIntermediateDemo.cs  —  EP.10  Interfaces
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  An interface is a CONTRACT — a list of members a class
//  PROMISES to provide. The interface defines WHAT; the class
//  decides HOW.
//
//  interface vs abstract class:
//    Interface     → pure contract, no state, no constructor
//                    a class can implement MANY interfaces
//    Abstract class → partial implementation, has state/fields
//                    a class can only inherit ONE abstract class
//
//  Key ideas:
//    1. Defining and implementing an interface
//    2. A class can implement MULTIPLE interfaces
//    3. Interface as a parameter type (polymorphism)
//    4. Explicit interface implementation (resolve name clash)
//    5. Default interface members (C# 8+)
//    6. Common built-in interfaces: IComparable, IEnumerable
//
//  Decision rule from Program.cs header:
//    "I need to treat different objects the same way
//     based on what they CAN DO" → Interface
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    // =========================================================
    //  Interfaces — the contracts
    // =========================================================

    // Any object that can take damage
    public interface IDamageable
    {
        int Health { get; }
        bool IsAlive { get; }
        void TakeDamage(int amount);
    }

    // Any object that can attack
    public interface IAttacker
    {
        int AttackPower { get; }
        string AttackStyle { get; }
        void Attack(IDamageable target);   // interface can reference another interface
    }

    // Any object that can be healed
    public interface IHealable
    {
        int MaxHealth { get; }
        void Heal(int amount);
    }

    // Interface with a DEFAULT implementation (C# 8+)
    // Classes get this for free unless they override it
    public interface IDescribable
    {
        string Name { get; }

        // Default method — optional to override
        string GetDescription() => $"{Name} [{GetType().Name}]";
    }

    // =========================================================
    //  Classes implementing multiple interfaces
    // =========================================================

    // Knight implements ALL four interfaces
    public class Knight : IDamageable, IAttacker, IHealable, IDescribable
    {
        public string Name { get; }
        public int Health { get; private set; }
        public int MaxHealth { get; }
        public bool IsAlive => Health > 0;
        public int AttackPower { get; }
        public string AttackStyle => "Sword";

        public Knight(string name, int maxHealth = 120, int attackPower = 25)
        {
            Name = name;
            MaxHealth = maxHealth;
            Health = maxHealth;
            AttackPower = attackPower;
        }

        public void TakeDamage(int amount)
        {
            Health = Math.Max(0, Health - amount);
            Console.WriteLine($"  {Name} takes {amount} dmg → HP {Health}/{MaxHealth}" +
                              (IsAlive ? "" : " [DEAD]"));
        }

        public void Attack(IDamageable target)
        {
            Console.WriteLine($"  {Name} slashes for {AttackPower}!");
            target.TakeDamage(AttackPower);
        }

        public void Heal(int amount)
        {
            int before = Health;
            Health = Math.Min(MaxHealth, Health + amount);
            Console.WriteLine($"  {Name} healed {Health - before} HP → {Health}/{MaxHealth}");
        }

        // Override the default interface method
        public string GetDescription() =>
            $"{Name} [Knight]  HP={Health}/{MaxHealth}  ATK={AttackPower}";
    }

    // Orc: IDamageable + IAttacker only (not healable)
    public class OrcWarrior : IDamageable, IAttacker, IDescribable
    {
        public string Name { get; }
        public int Health { get; private set; }
        public bool IsAlive => Health > 0;
        public int AttackPower { get; }
        public string AttackStyle => "Axe";

        public OrcWarrior(string name, int health = 200, int atk = 35)
        {
            Name = name;
            Health = health;
            AttackPower = atk;
        }

        public void TakeDamage(int amount)
        {
            Health = Math.Max(0, Health - amount);
            Console.WriteLine($"  {Name} grunts — takes {amount} dmg → HP {Health}" +
                              (IsAlive ? "" : " [DEFEATED]"));
        }

        public void Attack(IDamageable target)
        {
            Console.WriteLine($"  {Name} smashes with axe for {AttackPower}!");
            target.TakeDamage(AttackPower);
        }
        // GetDescription() — uses the DEFAULT from IDescribable (no override needed)
    }

    // HealingFountain: only IHealable — not a fighter
    public class HealingFountain : IHealable, IDescribable
    {
        public string Name { get; }
        public int MaxHealth => int.MaxValue;   // fountain never "fills up"

        public HealingFountain(string name) => Name = name;

        public void Heal(int amount)
        {
            // The fountain heals whoever uses it — caller decides the target
            Console.WriteLine($"  {Name} glows and restores {amount} HP.");
        }
    }

    // =========================================================
    //  Explicit interface implementation
    //  Use when two interfaces share a method name
    // =========================================================
    public interface IPrintable { void Print(); }
    public interface ILoggable { void Print(); }

    public class Report : IPrintable, ILoggable
    {
        public string Title { get; }
        public Report(string title) => Title = title;

        // Explicit — only callable through the interface reference
        void IPrintable.Print() => Console.WriteLine($"  [PRINT]  '{Title}' sent to printer.");
        void ILoggable.Print() => Console.WriteLine($"  [LOG]    '{Title}' written to log.");
    }

    // =========================================================
    //  Demo runner
    // =========================================================
    public static class InterfacesIntermediateDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Interfaces ===\n");

            BasicContractDemo();
            PolymorphismDemo();
            MultipleInterfacesDemo();
            ExplicitImplementationDemo();
            DefaultMemberDemo();
            InterfaceAsParameterDemo();
        }

        private static void BasicContractDemo()
        {
            Console.WriteLine("--- 1. Interface as Contract ---");
            Console.WriteLine("  IDamageable guarantees TakeDamage/Health/IsAlive exist.\n");

            var knight = new Knight("Arthur");
            Console.WriteLine($"  {knight.GetDescription()}");
            knight.TakeDamage(40);
            knight.Heal(15);
            knight.TakeDamage(120);
            Console.WriteLine($"  IsAlive: {knight.IsAlive}");
        }

        private static void PolymorphismDemo()
        {
            Console.WriteLine("\n--- 2. Polymorphism via Interface ---");
            Console.WriteLine("  A List<IDamageable> holds Knight AND OrcWarrior.\n");

            // Different types — same interface → same treatment
            var targets = new List<IDamageable>
            {
                new Knight("Lancelot", maxHealth: 100),
                new OrcWarrior("Gruk", health: 80),
                new Knight("Percival", maxHealth: 90),
            };

            Console.WriteLine("  Applying 30 damage to all targets:");
            foreach (IDamageable t in targets)
                t.TakeDamage(30);

            Console.WriteLine("\n  Survivors:");
            foreach (IDamageable t in targets)
                if (t.IsAlive) Console.WriteLine($"    → HP remaining: {t.Health}");
        }

        private static void MultipleInterfacesDemo()
        {
            Console.WriteLine("\n--- 3. A Class Implementing Multiple Interfaces ---");
            Console.WriteLine("  Knight is IDamageable + IAttacker + IHealable + IDescribable.\n");

            var arthur = new Knight("Arthur", maxHealth: 150, attackPower: 30);
            var gruk = new OrcWarrior("Gruk");

            // Use as IAttacker
            arthur.Attack(gruk);

            // Use as IHealable
            arthur.TakeDamage(50);
            arthur.Heal(30);

            // A reference typed as the interface — only that interface's members visible
            IHealable healable = arthur;
            healable.Heal(20);   // can only call Heal() — AttackPower not accessible here
        }

        private static void ExplicitImplementationDemo()
        {
            Console.WriteLine("\n--- 4. Explicit Interface Implementation ---");
            Console.WriteLine("  Resolves name clash when two interfaces share a method name.\n");

            var report = new Report("Q3 Earnings");

            // Must cast to the specific interface to reach explicit implementations
            ((IPrintable)report).Print();
            ((ILoggable)report).Print();

            // report.Print();  // ← compile error: ambiguous without explicit cast
        }

        private static void DefaultMemberDemo()
        {
            Console.WriteLine("\n--- 5. Default Interface Member (C# 8+) ---");
            Console.WriteLine("  OrcWarrior gets GetDescription() for free from IDescribable.\n");

            IDescribable orc = new OrcWarrior("Brutus");
            IDescribable knight = new Knight("Galahad");

            // Orc uses the DEFAULT implementation; Knight uses its OVERRIDE
            Console.WriteLine($"  Orc    → {orc.GetDescription()}");
            Console.WriteLine($"  Knight → {knight.GetDescription()}");
        }

        private static void InterfaceAsParameterDemo()
        {
            Console.WriteLine("\n--- 6. Interface as Parameter Type ---");
            Console.WriteLine("  HealUnit() works on ANY IHealable — decoupled from concrete type.\n");

            var arthur = new Knight("Arthur", maxHealth: 100);
            arthur.TakeDamage(60);

            HealUnit(arthur, 25);     // Knight is IHealable ✓

            var fountain = new HealingFountain("Ancient Fountain");
            HealUnit(fountain, 50);   // HealingFountain is IHealable ✓
        }

        // Accepts ANY IHealable — doesn't care what concrete type it is
        private static void HealUnit(IHealable unit, int amount)
        {
            Console.WriteLine($"  Healing {unit.GetType().Name}...");
            unit.Heal(amount);
        }
    }
}
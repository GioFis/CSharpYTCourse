// ============================================================
//  ClassIntermediateDemo.cs  —  EP.10  Class Intermediate
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  Beyond basic classes — the features that make OOP powerful.
//
//  Topics covered:
//    1. Constructor chaining         — this() calls another constructor
//    2. Static members               — shared across ALL instances
//    3. Inheritance                  — child class extends parent
//    4. Method overriding            — virtual / override / base
//    5. Abstract classes             — partial blueprint, can't be instantiated
//    6. sealed                       — prevent further inheritance
//    7. Object initializer syntax    — { Prop = value } shorthand
//
//  Key mental models:
//    static   → "belongs to the TYPE, not any instance"
//    virtual  → "parent says: child, you MAY replace this"
//    override → "child says: yes, I'm replacing it"
//    abstract → "parent says: child, you MUST implement this"
//    sealed   → "no one can inherit from me"
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    // =========================================================
    //  1. Constructor chaining + static members
    // =========================================================
    public class Enemy
    {
        // STATIC: shared counter — ONE value for the whole type
        private static int _totalCreated = 0;
        public static int TotalCreated => _totalCreated;

        // Instance fields
        public string Name { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }

        // Primary constructor — all params
        public Enemy(string name, int health, int damage)
        {
            Name = name;
            Health = health;
            Damage = damage;
            _totalCreated++;   // static counter incremented on every new instance
        }

        // Chained constructor — calls the primary via this()
        // "Create a default enemy with just a name"
        public Enemy(string name) : this(name, health: 100, damage: 10) { }

        // No-arg constructor — calls the chained one
        public Enemy() : this("Unknown") { }

        // Static factory method — another common pattern
        public static Enemy CreateBoss(string name) =>
            new Enemy(name, health: 500, damage: 50);

        // Virtual method — child classes CAN override this
        public virtual string Describe() =>
            $"[Enemy] {Name}  HP={Health}  DMG={Damage}";

        // Virtual — child may override
        public virtual void Attack(string target) =>
            Console.WriteLine($"  {Name} attacks {target} for {Damage} damage.");
    }

    // =========================================================
    //  2. Inheritance + override
    // =========================================================
    public class Goblin : Enemy
    {
        public int StealthLevel { get; set; }

        // Call parent constructor with : base(...)
        public Goblin(string name, int stealthLevel)
            : base(name, health: 60, damage: 15)
        {
            StealthLevel = stealthLevel;
        }

        // override — replaces the parent's virtual method
        public override string Describe() =>
            $"[Goblin] {Name}  HP={Health}  DMG={Damage}  Stealth={StealthLevel}";

        // override + base — extend, don't fully replace
        public override void Attack(string target)
        {
            if (StealthLevel > 5)
                Console.WriteLine($"  {Name} sneaks up and backstabs {target}!");
            else
                base.Attack(target);   // fall back to parent behaviour
        }
    }

    public class Dragon : Enemy
    {
        public string Element { get; set; }

        public Dragon(string name, string element)
            : base(name, health: 800, damage: 120)
        {
            Element = element;
        }

        public override string Describe() =>
            $"[Dragon] {Name}  HP={Health}  DMG={Damage}  Element={Element}";

        public override void Attack(string target) =>
            Console.WriteLine($"  {Name} breathes {Element} fire at {target}! ({Damage} dmg)");
    }

    // =========================================================
    //  3. Abstract class — partial blueprint
    //     Cannot be instantiated directly.
    //     Forces subclasses to implement abstract members.
    // =========================================================
    public abstract class Shape
    {
        public string Color { get; set; } = "White";

        // Abstract — NO body here; subclass MUST implement
        public abstract double Area();
        public abstract double Perimeter();

        // Concrete method — inherited as-is (or overridden)
        public virtual void Describe()
        {
            Console.WriteLine($"  {GetType().Name}: color={Color}  " +
                              $"area={Area():F2}  perimeter={Perimeter():F2}");
        }
    }

    public class Circle : Shape
    {
        public double Radius { get; set; }
        public Circle(double radius, string color = "White")
        {
            Radius = radius;
            Color = color;
        }
        public override double Area() => Math.PI * Radius * Radius;
        public override double Perimeter() => 2 * Math.PI * Radius;
    }

    public class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public Rectangle(double width, double height, string color = "White")
        {
            Width = width;
            Height = height;
            Color = color;
        }
        public override double Area() => Width * Height;
        public override double Perimeter() => 2 * (Width + Height);

        // Extra override — add dimensions to the description
        public override void Describe()
        {
            base.Describe();   // call parent's version first
            Console.WriteLine($"    → {Width}×{Height}");
        }
    }

    // =========================================================
    //  4. sealed — no further inheritance allowed
    // =========================================================
    public sealed class FinalBoss : Dragon
    {
        public int Phase { get; private set; } = 1;

        public FinalBoss(string name) : base(name, "Void") { }

        public void NextPhase()
        {
            Phase++;
            Damage = (int)(Damage * 1.5);
            Console.WriteLine($"  {Name} enters PHASE {Phase}! Damage → {Damage}");
        }

        public override string Describe() =>
            $"[FinalBoss ★] {Name}  HP={Health}  DMG={Damage}  Phase={Phase}";
    }
    // public class UltraBoss : FinalBoss { }  // ← compile error: sealed!

    // =========================================================
    //  Demo runner
    // =========================================================
    public static class ClassIntermediateDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Class Intermediate ===\n");

            ConstructorChainingDemo();
            StaticMembersDemo();
            InheritanceDemo();
            AbstractClassDemo();
            SealedDemo();
            ObjectInitializerDemo();
        }

        private static void ConstructorChainingDemo()
        {
            Console.WriteLine("--- 1. Constructor Chaining ---");
            Console.WriteLine("  Each constructor delegates to the next via this(...):\n");

            var e1 = new Enemy();                        // no-arg → chains to name-only → chains to full
            var e2 = new Enemy("Skeleton");              // name-only → chains to full
            var e3 = new Enemy("Troll", 200, 25);        // full constructor directly
            var e4 = Enemy.CreateBoss("Demon Lord");     // static factory

            Console.WriteLine($"  {e1.Describe()}");
            Console.WriteLine($"  {e2.Describe()}");
            Console.WriteLine($"  {e3.Describe()}");
            Console.WriteLine($"  {e4.Describe()}");
        }

        private static void StaticMembersDemo()
        {
            Console.WriteLine("\n--- 2. Static Members ---");
            Console.WriteLine("  Static = belongs to the TYPE, not any one instance.\n");

            Console.WriteLine($"  Enemies created so far: {Enemy.TotalCreated}");
            _ = new Enemy("Rat");
            _ = new Enemy("Bat");
            Console.WriteLine($"  After two more: {Enemy.TotalCreated}");
            Console.WriteLine("  Every instance shares the same _totalCreated counter.");
        }

        private static void InheritanceDemo()
        {
            Console.WriteLine("\n--- 3. Inheritance + virtual/override/base ---\n");

            // Polymorphism: list of base type holds different subtypes
            var enemies = new List<Enemy>
            {
                new Enemy("Slime", 30, 5),
                new Goblin("Sneaky", stealthLevel: 8),
                new Goblin("Stumpy", stealthLevel: 2),
                new Dragon("Inferno", "Magma"),
            };

            foreach (var e in enemies)
            {
                Console.WriteLine(e.Describe());   // virtual dispatch → correct override called
                e.Attack("Hero");
            }
        }

        private static void AbstractClassDemo()
        {
            Console.WriteLine("\n--- 4. Abstract Class ---");
            Console.WriteLine("  Shape cannot be instantiated; Circle and Rectangle must implement Area/Perimeter.\n");

            // new Shape();  // ← compile error: abstract!

            var shapes = new List<Shape>
            {
                new Circle(5, "Red"),
                new Rectangle(4, 7, "Blue"),
                new Circle(2.5),
            };

            foreach (var s in shapes)
                s.Describe();   // virtual dispatch — Rectangle adds its extra line
        }

        private static void SealedDemo()
        {
            Console.WriteLine("\n--- 5. sealed class ---");
            Console.WriteLine("  FinalBoss cannot be subclassed.\n");

            var boss = new FinalBoss("Void King");
            Console.WriteLine($"  {boss.Describe()}");
            boss.Attack("Party");
            boss.NextPhase();
            boss.NextPhase();
            Console.WriteLine($"  {boss.Describe()}");
        }

        private static void ObjectInitializerDemo()
        {
            Console.WriteLine("\n--- 6. Object Initializer Syntax ---");
            Console.WriteLine("  Set properties inline without a matching constructor:\n");

            // No constructor needed for these properties
            var goblin = new Goblin("Imp", stealthLevel: 3)
            {
                Health = 40,       // override what the constructor set
                Damage = 8,
            };

            Console.WriteLine($"  {goblin.Describe()}");

            // Works with any class — very common for DTOs / config objects
            var rect = new Rectangle(10, 5) { Color = "Gold" };
            rect.Describe();
        }
    }
}
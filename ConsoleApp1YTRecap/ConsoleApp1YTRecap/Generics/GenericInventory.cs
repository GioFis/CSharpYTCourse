// ============================================================
//  GenericInventory.cs  —  GENERIC CLASS + SUPPORTING TYPES
//
//  Code Monkey's core teaching on Generics:
//  ─────────────────────────────────────────
//  A Generic is a CLASS or METHOD with a TYPE PLACEHOLDER, written <T>.
//  You write the logic ONCE and the compiler stamps out a type-safe
//  version for whatever T you pick at the call site.
//
//  The classic pain point he shows:
//    • You build an Inventory<Sword>
//    • Then you need Inventory<Potion>
//    • Without generics → copy-paste the whole class, change the type
//    • With generics    → one class, works for everything, zero duplication
//
//  Unity analogy: a generic "wrapper" or "slot" that can hold any
//  MonoBehaviour — e.g. a UI slot that shows either a weapon or a spell.
//  Console analogy here: an Inventory that can hold Weapons OR Potions.
//
//  Naming convention Code Monkey follows:
//    T       — one type param (most common)
//    TKey, TValue — two params (like Dictionary<TKey,TValue>)
//    where T : ISomething  — constraint: T must honour a contract
// ============================================================

namespace ConsoleApp1YTRecap.Generics
{
    // ── DOMAIN TYPES — the things we'll put inside the generic inventory ──────

    // A simple weapon. Note: it knows nothing about generics — it's just data.
    public class Weapon
    {
        public string Name { get; }
        public int Damage { get; }

        public Weapon(string name, int damage)
        {
            Name = name;
            Damage = damage;
        }

        // ToString() lets Console.WriteLine print something readable
        public override string ToString() => $"[Weapon] {Name} (dmg:{Damage})";
    }

    // A potion — completely unrelated to Weapon in the class hierarchy.
    // The fact that the same Inventory class can hold BOTH is the whole point.
    public class Potion
    {
        public string Name { get; }
        public int Heal { get; }

        public Potion(string name, int heal)
        {
            Name = name;
            Heal = heal;
        }

        public override string ToString() => $"[Potion] {Name} (heal:{Heal})";
    }

    // A gold coin — yet another unrelated type.
    public class GoldCoin
    {
        public int Value { get; }
        public GoldCoin(int value) { Value = value; }
        public override string ToString() => $"[Gold] {Value} gp";
    }

    // ── THE GENERIC CLASS ─────────────────────────────────────────────────────
    //
    //  Inventory<T>  — T is the placeholder.
    //  When you write  new Inventory<Weapon>(4)  the compiler replaces
    //  every T in this class with Weapon. It's as if you wrote
    //  a class specifically for Weapon, but you only had to write it once.
    //
    //  <T> goes right after the class name, just like the video shows.

    public class Inventory<T>
    {
        // _slots is an array of T — could be Weapon[], Potion[], anything.
        private T[] _slots;

        // _count tracks how many items are currently stored.
        private int _count;

        // The constructor takes a capacity — how many slots the bag has.
        public Inventory(int capacity)
        {
            _slots = new T[capacity];
            _count = 0;
        }

        // ── Add ───────────────────────────────────────────────────────────────
        // T item — the parameter type is T, so it will only accept whatever
        // type this Inventory was created with. Try passing a Potion to
        // Inventory<Weapon> and you get a COMPILE-TIME error — not a crash.
        // That type-safety is one of generics' biggest wins.
        public bool Add(T item)
        {
            if (_count >= _slots.Length)
            {
                Console.WriteLine($"  ⚠  Inventory full! Could not add {item}");
                return false;
            }
            _slots[_count] = item;
            _count++;
            Console.WriteLine($"  + Added  {item}  (slot {_count}/{_slots.Length})");
            return true;
        }

        // ── Get ───────────────────────────────────────────────────────────────
        // Returns T — again, the return type matches whatever T is.
        // No casting needed at the call site; the compiler knows the type.
        public T Get(int index)
        {
            if (index < 0 || index >= _count)
            {
                // default(T) is null for reference types, 0 for value types.
                // Code Monkey mentions default(T) as a safe "nothing" value.
                Console.WriteLine($"  ⚠  Index {index} out of range. Returning default.");
                return default(T);
            }
            return _slots[index];
        }

        // ── Count ─────────────────────────────────────────────────────────────
        public int Count => _count;

        // ── Print all ─────────────────────────────────────────────────────────
        // Works for any T because we only call ToString(), which every object has.
        public void PrintAll(string label)
        {
            Console.WriteLine($"\n  📦 {label} ({_count}/{_slots.Length} used):");
            for (int i = 0; i < _count; i++)
                Console.WriteLine($"    [{i}] {_slots[i]}");
            if (_count == 0)
                Console.WriteLine("    (empty)");
        }
    }

    // ── GENERIC METHOD (standalone helper class) ───────────────────────────────
    //
    //  Code Monkey also shows generic METHODS — not just generic classes.
    //  A generic method has its own <T> right after the method name.
    //  It can live in a perfectly normal (non-generic) class.

    public static class InventoryUtils
    {
        // Swap two items — works for any type, no duplication.
        // The compiler infers T from what you pass in.
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        // Print any single value with a label — generic method, one line.
        public static void Print<T>(string label, T value)
        {
            Console.WriteLine($"  {label}: {value}");
        }
    }

    // ── CONSTRAINED GENERIC ────────────────────────────────────────────────────
    //
    //  Code Monkey's next step: "what if T needs to do something specific?"
    //  Answer: where T : ISomeInterface  — a CONSTRAINT.
    //  Now T must honour that contract, so you can call those methods safely.
    //
    //  We reuse IAttackable from EP.6 — anything attackable can go in a
    //  BattleSlot<T> and the slot can read its Health without casting.

    public class BattleSlot<T> where T : Interfaces.IAttackable
    {
        // Because of the constraint, the compiler KNOWS T has .Health
        // and .TakeDamage() — no casting, no guessing.
        public T Occupant { get; private set; }

        public BattleSlot(T occupant)
        {
            Occupant = occupant;
            Console.WriteLine($"  🏟  Battle slot filled with {occupant} (HP: {occupant.Health})");
        }

        // This method is only possible because of the 'where T : IAttackable' constraint.
        // Without it the compiler would refuse to call .TakeDamage() on an unknown T.
        public void ApplyDot(int damagePerTick, int ticks)
        {
            Console.WriteLine($"\n  ⏱  Applying {damagePerTick} dmg/tick for {ticks} ticks:");
            for (int i = 1; i <= ticks; i++)
            {
                Occupant.TakeDamage(damagePerTick);
                if (Occupant.Health <= 0) break;
            }
        }
    }
}
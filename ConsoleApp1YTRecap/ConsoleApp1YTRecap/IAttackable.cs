// ============================================================
//  IAttackable.cs  —  INTERFACE DEFINITION + IMPLEMENTING TYPES
//
//  Code Monkey's core teaching:
//  An interface is a CONTRACT. It says "I promise I have these methods."
//  It doesn't care HOW you implement them — just THAT you do.
//
//  Unity analogy → IInteractable: anything the player can interact with.
//  Our analogy  → IAttackable:  anything in a game world that can be attacked.
//
//  The power: a single method Attack(IAttackable target) works on
//  Hero, Monster, Chest, Tower — things with NOTHING else in common.
// ============================================================

namespace TryPlay1
{
    // ── THE CONTRACT ──────────────────────────────────────────────────────────
    // Any class that implements IAttackable MUST provide these two members.
    // No fields, no constructors — just the shape of the promise.

    public interface IAttackable
    {
        int Health { get; }          // read-only property
        void TakeDamage(int amount);   // method signature — no body
    }

    // ── IMPLEMENTORS — very different classes, same contract ──────────────────

    public class Hero : IAttackable
    {
        public string Name { get; }
        public int Health { get; private set; }

        public Hero(string name, int health)
        {
            Name = name;
            Health = health;
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            Console.WriteLine($"  ⚔  Hero {Name} takes {amount} damage → HP: {Health}");
            if (Health <= 0)
                Console.WriteLine($"  💀 {Name} has fallen!");
        }
    }

    public class Monster : IAttackable
    {
        public string Name { get; }
        public int Health { get; private set; }

        public Monster(string name, int health)
        {
            Name = name;
            Health = health;
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            Console.WriteLine($"  🐉 Monster [{Name}] takes {amount} damage → HP: {Health}");
            if (Health <= 0)
                Console.WriteLine($"  💥 {Name} has been slain!");
        }
    }

    // A chest isn't a creature — it has nothing in common with Hero/Monster
    // except: it CAN be attacked (smashed open).
    public class TreasureChest : IAttackable
    {
        public string Name { get; }
        public int Health { get; private set; }   // "durability"

        public TreasureChest(string name)
        {
            Name = name;
            Health = 30;
        }

        public void TakeDamage(int amount)
        {
            Health -= amount;
            Console.WriteLine($"  📦 Chest [{Name}] is hit for {amount} → Durability: {Health}");
            if (Health <= 0)
                Console.WriteLine($"  🎁 {Name} breaks open — loot spills out!");
        }
    }

    // ── THE ATTACKER — knows nothing about Hero/Monster/Chest ─────────────────
    // It only knows the CONTRACT: whatever you give me has TakeDamage().

    public class Warrior
    {
        public string Name { get; }
        private int _attackPower;

        public Warrior(string name, int attackPower)
        {
            Name = name;
            _attackPower = attackPower;
        }

        // This method works for ANY IAttackable — past, present, future.
        // You could add a Dragon class tomorrow and this never changes.
        public void Attack(IAttackable target)
        {
            Console.WriteLine($"\n  [{Name}] attacks!");
            target.TakeDamage(_attackPower);
        }
    }
}
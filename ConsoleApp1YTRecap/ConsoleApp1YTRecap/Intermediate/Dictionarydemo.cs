// ============================================================
//  DictionaryDemo.cs  —  EP.10  Dictionary
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  A Dictionary<TKey, TValue> stores KEY → VALUE pairs.
//  Looking up a value by key is O(1) — instant, regardless
//  of how many items are stored (uses a hash table internally).
//
//  Compare to an array/list:
//    List   → find by index (int)          arr[2]
//    Dict   → find by any key (string, int, enum…)  dict["apple"]
//
//  Rules:
//    • Every KEY must be UNIQUE in the dictionary
//    • Values can repeat
//    • Keys and values can be any type (including custom classes)
//
//  Key operations:
//    Add / update    dict[key] = value
//    Safe read       dict.TryGetValue(key, out value)
//    Remove          dict.Remove(key)
//    Check key       dict.ContainsKey(key)
//    Iterate         foreach (var kvp in dict)  → kvp.Key, kvp.Value
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    public static class DictionaryDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Dictionary ===\n");

            BasicOperations();
            SafeAccess();
            FrequencyCounter();
            DictionaryOfObjects();
            GroupingWithDictionary();
        }

        // ── 1. Basic CRUD operations ──────────────────────────────────────
        private static void BasicOperations()
        {
            Console.WriteLine("--- 1. Basic Operations ---");

            // Create — specify key type and value type
            var capitals = new Dictionary<string, string>();

            // ADD with dict[key] = value  (also works for updates)
            capitals["Italy"] = "Rome";
            capitals["France"] = "Paris";
            capitals["Germany"] = "Berlin";
            capitals["Spain"] = "Madrid";

            Console.WriteLine($"Count: {capitals.Count}");

            // READ — direct indexer (throws KeyNotFoundException if missing)
            Console.WriteLine($"Capital of Italy:   {capitals["Italy"]}");

            // UPDATE — same syntax as add; key already exists → overwrites
            capitals["Germany"] = "Berlin (updated)";
            Console.WriteLine($"Capital of Germany: {capitals["Germany"]}");

            // REMOVE
            capitals.Remove("Spain");
            Console.WriteLine($"After removing Spain, count: {capitals.Count}");

            // CHECK
            Console.WriteLine($"ContainsKey France: {capitals.ContainsKey("France")}");
            Console.WriteLine($"ContainsKey Spain:  {capitals.ContainsKey("Spain")}");

            // ITERATE — KeyValuePair<TKey,TValue> (kvp is the convention)
            Console.WriteLine("\nAll entries:");
            foreach (var kvp in capitals)
                Console.WriteLine($"  {kvp.Key,-10} → {kvp.Value}");

            // Iterate keys or values only
            Console.WriteLine("\nKeys only:   " + string.Join(", ", capitals.Keys));
            Console.WriteLine("Values only: " + string.Join(", ", capitals.Values));
        }

        // ── 2. Safe access with TryGetValue ──────────────────────────────
        // Direct indexer on a missing key THROWS. TryGetValue never throws.
        private static void SafeAccess()
        {
            Console.WriteLine("\n--- 2. Safe Access: TryGetValue ---");

            var scores = new Dictionary<string, int>
            {
                { "Alice", 95 },
                { "Bob",   82 },
                // initializer syntax: { key, value }
            };

            string[] names = { "Alice", "Charlie", "Bob" };

            foreach (string name in names)
            {
                // TryGetValue: returns true if found, puts value in 'out' variable
                if (scores.TryGetValue(name, out int score))
                    Console.WriteLine($"  {name}: {score}");
                else
                    Console.WriteLine($"  {name}: not found (no exception!)");
            }

            // GetValueOrDefault — returns default(T) or your fallback if missing
            int aliceScore = scores.GetValueOrDefault("Alice", 0);
            int unknownScore = scores.GetValueOrDefault("Zara", -1);
            Console.WriteLine($"\n  Alice (default 0):   {aliceScore}");
            Console.WriteLine($"  Zara  (default -1):  {unknownScore}");
        }

        // ── 3. Counting frequencies ───────────────────────────────────────
        // Classic dictionary pattern: key = item, value = count.
        private static void FrequencyCounter()
        {
            Console.WriteLine("\n--- 3. Word Frequency Counter ---");
            Console.WriteLine("  Pattern: dict[word]++ to count occurrences.\n");

            string sentence = "the cat sat on the mat and the cat wore a hat";
            string[] words = sentence.Split(' ');

            var freq = new Dictionary<string, int>();

            foreach (string word in words)
            {
                // If key exists: increment. If not: start at 0 then add 1.
                if (freq.ContainsKey(word))
                    freq[word]++;
                else
                    freq[word] = 1;

                // Shorter idiom (same effect):
                // freq[word] = freq.GetValueOrDefault(word, 0) + 1;
            }

            // Sort by frequency descending before printing
            foreach (var kvp in freq.OrderByDescending(k => k.Value))
                Console.WriteLine($"  '{kvp.Key}' appears {kvp.Value}x");
        }

        // ── 4. Dictionary with object values ─────────────────────────────
        // Values can be any type — including custom classes.
        private static void DictionaryOfObjects()
        {
            Console.WriteLine("\n--- 4. Dictionary<string, Player> ---");

            var players = new Dictionary<string, Player>
            {
                ["p001"] = new Player("Alice", 10),
                ["p002"] = new Player("Bob", 7),
                ["p003"] = new Player("Carlo", 15),
            };

            // Look up and modify
            if (players.TryGetValue("p002", out Player bob))
            {
                bob.Level++;
                Console.WriteLine($"  {bob.Name} levelled up to {bob.Level}");
            }

            Console.WriteLine("\n  All players:");
            foreach (var (id, player) in players)   // deconstruct kvp inline
                Console.WriteLine($"  {id}: {player.Name} — Level {player.Level}");
        }

        // ── 5. Grouping with Dictionary<string, List<T>> ─────────────────
        // Value can itself be a collection — common for grouping.
        private static void GroupingWithDictionary()
        {
            Console.WriteLine("\n--- 5. Grouping: Dictionary<string, List<string>> ---");

            string[] items = { "apple", "avocado", "banana", "blueberry",
                               "cherry", "apricot", "cranberry", "blackberry" };

            var byLetter = new Dictionary<string, List<string>>();

            foreach (string item in items)
            {
                string key = item[0].ToString().ToUpper();   // first letter as key

                if (!byLetter.ContainsKey(key))
                    byLetter[key] = new List<string>();       // create list on first encounter

                byLetter[key].Add(item);
            }

            foreach (var kvp in byLetter.OrderBy(k => k.Key))
                Console.WriteLine($"  {kvp.Key}: {string.Join(", ", kvp.Value)}");
        }

        // ── Simple model used in section 4 ───────────────────────────────
        private class Player
        {
            public string Name { get; }
            public int Level { get; set; }
            public Player(string name, int level) { Name = name; Level = level; }
        }
    }
}
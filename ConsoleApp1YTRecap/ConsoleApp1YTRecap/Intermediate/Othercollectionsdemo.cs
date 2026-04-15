// ============================================================
//  OtherCollectionsDemo.cs  —  EP.10  Other Collections
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  Beyond List<T> and Dictionary<K,V>, C# ships several
//  specialised collections — each optimised for one job.
//
//  Covered here:
//    HashSet<T>      — unique items, fast membership check
//    Stack<T>        — Last-In First-Out  (LIFO)
//    Queue<T>        — First-In First-Out (FIFO)
//    LinkedList<T>   — cheap insert/remove anywhere in sequence
//
//  Choosing the right collection avoids unnecessary loops
//  and makes the CODE self-documenting ("oh, it's a Queue,
//  so items are processed in order").
// ============================================================

namespace ConsoleApp1YTRecap.Intermediate
{
    public static class OtherCollectionsDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Other Collections ===\n");

            HashSetDemo();
            StackDemo();
            QueueDemo();
            LinkedListDemo();
            ChoosingGuide();
        }

        // ── 1. HashSet<T> ─────────────────────────────────────────────────
        // Stores UNIQUE items. Duplicate Add() calls are silently ignored.
        // Contains() is O(1) — much faster than List.Contains() which is O(n).
        //
        // Use when: you care about membership, not order or duplicates.
        private static void HashSetDemo()
        {
            Console.WriteLine("--- 1. HashSet<T> — unique items, O(1) lookup ---");

            var visited = new HashSet<string>();

            // Add returns false (and does nothing) if the item already exists
            string[] pages = { "Home", "About", "Home", "Shop", "About", "Contact" };
            foreach (string page in pages)
            {
                bool added = visited.Add(page);
                Console.WriteLine($"  Add '{page}': {(added ? "✓ new" : "✗ already there")}");
            }

            Console.WriteLine($"\n  Unique pages visited: {visited.Count}");
            Console.WriteLine($"  Contains 'Shop': {visited.Contains("Shop")}");
            Console.WriteLine($"  Contains 'Blog': {visited.Contains("Blog")}");

            // Set operations — very useful for comparing groups
            var premiumPages = new HashSet<string> { "Shop", "Checkout", "Account" };

            // IntersectWith: keep only items in BOTH sets
            var both = new HashSet<string>(visited);
            both.IntersectWith(premiumPages);
            Console.WriteLine($"\n  Pages visited that are premium: {string.Join(", ", both)}");

            // UnionWith: combine both sets, still unique
            var all = new HashSet<string>(visited);
            all.UnionWith(premiumPages);
            Console.WriteLine($"  Union (all unique pages): {string.Join(", ", all)}");

            // ExceptWith: remove items found in the other set
            var onlyFree = new HashSet<string>(visited);
            onlyFree.ExceptWith(premiumPages);
            Console.WriteLine($"  Free pages visited:       {string.Join(", ", onlyFree)}");
        }

        // ── 2. Stack<T> ───────────────────────────────────────────────────
        // LIFO — the last item pushed is the first item popped.
        // Think: a stack of plates. You always take from the TOP.
        //
        // Use when: undo/redo, call-stack simulation, depth-first search,
        //           expression parsing, backtracking.
        private static void StackDemo()
        {
            Console.WriteLine("\n--- 2. Stack<T> — Last-In First-Out (LIFO) ---");
            Console.WriteLine("  Imagine: undo history in a text editor.\n");

            var undoHistory = new Stack<string>();

            // Push = add to the top
            string[] actions = { "Type 'Hello'", "Bold selection", "Insert image", "Delete word" };
            foreach (string action in actions)
            {
                undoHistory.Push(action);
                Console.WriteLine($"  Do:   '{action}'  (stack size: {undoHistory.Count})");
            }

            Console.WriteLine($"\n  Peek (see top without removing): '{undoHistory.Peek()}'");
            Console.WriteLine($"  Stack size still: {undoHistory.Count}");

            // Pop = remove from the top (LIFO order)
            Console.WriteLine("\n  Undo (Pop) three times:");
            for (int i = 0; i < 3; i++)
            {
                string undone = undoHistory.Pop();
                Console.WriteLine($"  Undo: '{undone}'  (remaining: {undoHistory.Count})");
            }

            Console.WriteLine($"\n  TryPop on empty stack:");
            undoHistory.Clear();
            bool gotItem = undoHistory.TryPop(out string result);
            Console.WriteLine($"  TryPop returned: {gotItem} — safe, no exception.");
        }

        // ── 3. Queue<T> ───────────────────────────────────────────────────
        // FIFO — the first item enqueued is the first item dequeued.
        // Think: a real queue / waiting line. First come, first served.
        //
        // Use when: print spooler, task scheduler, BFS, message bus.
        private static void QueueDemo()
        {
            Console.WriteLine("\n--- 3. Queue<T> — First-In First-Out (FIFO) ---");
            Console.WriteLine("  Imagine: a printer queue.\n");

            var printQueue = new Queue<string>();

            // Enqueue = join the back of the line
            string[] docs = { "Report.pdf", "Invoice.docx", "Photo.png" };
            foreach (string doc in docs)
            {
                printQueue.Enqueue(doc);
                Console.WriteLine($"  Queued: '{doc}'  (queue size: {printQueue.Count})");
            }

            Console.WriteLine($"\n  Peek (next to print): '{printQueue.Peek()}'");

            // Dequeue = take from the front (FIFO order)
            Console.WriteLine("\n  Printing documents:");
            while (printQueue.Count > 0)
            {
                string doc = printQueue.Dequeue();
                Console.WriteLine($"  Printing: '{doc}'  (remaining: {printQueue.Count})");
            }

            // TryDequeue is the safe version (no exception on empty queue)
            bool got = printQueue.TryDequeue(out string next);
            Console.WriteLine($"\n  TryDequeue on empty queue: {got} — no exception.");
        }

        // ── 4. LinkedList<T> ─────────────────────────────────────────────
        // Each item (node) holds a value + pointers to Previous and Next.
        // Insert or remove anywhere in O(1) once you have the node —
        // no shifting of elements like in a List.
        //
        // Use when: frequent insert/remove in the MIDDLE of a sequence;
        //           e.g., playlist, token stream, task priority queue.
        private static void LinkedListDemo()
        {
            Console.WriteLine("\n--- 4. LinkedList<T> — cheap insert/remove anywhere ---");
            Console.WriteLine("  Imagine: a music playlist.\n");

            var playlist = new LinkedList<string>();

            // AddLast / AddFirst
            playlist.AddLast("Song A");
            playlist.AddLast("Song B");
            playlist.AddLast("Song C");
            playlist.AddFirst("Intro");    // goes to the very front

            PrintPlaylist(playlist);

            // Find a node and insert BEFORE or AFTER it — no index needed
            var nodeB = playlist.Find("Song B");
            if (nodeB != null)
            {
                playlist.AddAfter(nodeB, "Song B2 (bonus)");
                playlist.AddBefore(nodeB, "Song A.5");
            }

            Console.WriteLine("\nAfter inserting around 'Song B':");
            PrintPlaylist(playlist);

            // Remove by value or by node
            playlist.Remove("Intro");
            Console.WriteLine("\nAfter removing 'Intro':");
            PrintPlaylist(playlist);

            // Navigate with First / Last and the .Next / .Previous pointers
            Console.WriteLine($"\n  First: {playlist.First?.Value}");
            Console.WriteLine($"  Last:  {playlist.Last?.Value}");
            Console.WriteLine($"  Second item: {playlist.First?.Next?.Value}");
        }

        private static void PrintPlaylist(LinkedList<string> list)
        {
            int i = 1;
            foreach (string song in list)
                Console.WriteLine($"  {i++}. {song}");
        }

        // ── 5. Quick-reference guide ──────────────────────────────────────
        private static void ChoosingGuide()
        {
            Console.WriteLine("\n--- Choosing the Right Collection ---\n");

            var rows = new[]
            {
                ("List<T>",        "Ordered, duplicates OK",      "General purpose, index access"),
                ("Dictionary<K,V>","Key→Value pairs",             "Fast lookup by key"),
                ("HashSet<T>",     "Unique items, no order",      "Membership test, de-duplication"),
                ("Stack<T>",       "LIFO (last in, first out)",   "Undo, DFS, backtracking"),
                ("Queue<T>",       "FIFO (first in, first out)",  "Task queue, BFS, print spooler"),
                ("LinkedList<T>",  "Doubly linked nodes",         "Frequent mid-list insert/remove"),
            };

            Console.WriteLine($"  {"Type",-20} {"Characteristic",-30} {"Best for"}");
            Console.WriteLine("  " + new string('-', 75));
            foreach (var (type, trait, use) in rows)
                Console.WriteLine($"  {type,-20} {trait,-30} {use}");
        }
    }
}
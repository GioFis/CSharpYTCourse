// ============================================================
//  NamespacesDemo.cs  —  EP.10  Namespaces
//  Namespace: ConsoleApp1YTRecap.Intermediate
//
//  WHAT THIS SHOWS:
//  A namespace is a LOGICAL CONTAINER for related types.
//  It prevents naming conflicts and organises code into
//  meaningful, navigable groups.
//
//  Key ideas:
//    1. Declaring namespaces — traditional and file-scoped (C# 10)
//    2. Nested namespaces   — hierarchy mirrors folder structure
//    3. using directive     — import a namespace, avoid full names
//    4. using alias         — rename a type/namespace at the use site
//    5. global using        — project-wide import (C# 10, in one file)
//    6. Conflict resolution — fully qualified name when two types clash
//    7. namespace + folder  — convention: one namespace per folder
//
//  Convention (matches this project):
//    CompanyName.ProjectName.FeatureArea
//    e.g. ConsoleApp1YTRecap.Intermediate
//
//  File-scoped namespace (C# 10+):
//    namespace MyApp.Utils;   ← no braces, entire file is in this namespace
//    (All files in this demo already use this style)
// ============================================================

// Simulated "external" types defined inline to keep this self-contained.
// In a real project each would live in its own file in the matching folder.

namespace ConsoleApp1YTRecap.GameEngine.Physics
{
    public class Vector2 { public float X, Y; public override string ToString() => $"({X},{Y})"; }
    public class Rigidbody { public float Mass { get; set; } = 1f; public override string ToString() => $"Rigidbody(mass={Mass})"; }
}

namespace ConsoleApp1YTRecap.GameEngine.Rendering
{
    public class Texture { public string Name { get; set; } = "default"; }
    public class Camera { public float Fov { get; set; } = 60f; public override string ToString() => $"Camera(fov={Fov})"; }
    // Intentional name clash with Physics.Vector2 — used in conflict demo
    public class Vector2 { public float U, V; public override string ToString() => $"UV({U},{V})"; }
}

namespace ConsoleApp1YTRecap.Utilities
{
    public static class MathHelper
    {
        public static float Clamp(float v, float min, float max) => Math.Clamp(v, min, max);
        public static float Lerp(float a, float b, float t) => a + (b - a) * t;
    }

    public static class StringHelper
    {
        public static string Truncate(string s, int maxLen) =>
            s.Length <= maxLen ? s : s[..maxLen] + "…";
    }
}

namespace ConsoleApp1YTRecap.Intermediate
{
    // ── 'using' brings in the namespaces we need ──────────────────────────
    using ConsoleApp1YTRecap.GameEngine.Physics;
    using ConsoleApp1YTRecap.Utilities;

    // Alias — rename at the use site to avoid the clash with Rendering.Vector2
    using PhysVec = ConsoleApp1YTRecap.GameEngine.Physics.Vector2;
    using RenderVec = ConsoleApp1YTRecap.GameEngine.Rendering.Vector2;

    public static class NamespacesDemo
    {
        public static void Run()
        {
            Console.WriteLine("=== EP.10 Namespaces ===\n");

            BasicNamespaceDemo();
            UsingAliasDemo();
            ConflictResolutionDemo();
            ConventionGuide();
        }

        // ── 1. Declaring + using namespaces ───────────────────────────────
        private static void BasicNamespaceDemo()
        {
            Console.WriteLine("--- 1. Using types from other namespaces ---\n");

            // 'using ConsoleApp1YTRecap.GameEngine.Physics' at top → no full name needed
            var v = new Vector2 { X = 3f, Y = 4f };
            var rb = new Rigidbody { Mass = 2.5f };

            Console.WriteLine($"  Physics.Vector2:   {v}");
            Console.WriteLine($"  Physics.Rigidbody: {rb}");

            // 'using ConsoleApp1YTRecap.Utilities'
            float clamped = MathHelper.Clamp(150f, 0f, 100f);
            float lerped = MathHelper.Lerp(0f, 100f, 0.25f);
            Console.WriteLine($"\n  MathHelper.Clamp(150, 0, 100) = {clamped}");
            Console.WriteLine($"  MathHelper.Lerp(0, 100, 0.25) = {lerped}");

            string truncated = StringHelper.Truncate("Hello, World! This is long.", 12);
            Console.WriteLine($"  StringHelper.Truncate(...)    = \"{truncated}\"");
        }

        // ── 2. using alias ────────────────────────────────────────────────
        // Rename a type for clarity or to resolve ambiguity.
        private static void UsingAliasDemo()
        {
            Console.WriteLine("\n--- 2. using alias ---");
            Console.WriteLine("  PhysVec and RenderVec alias two different Vector2 types.\n");

            var physVec = new PhysVec { X = 1f, Y = 2f };
            var renderVec = new RenderVec { U = 0.5f, V = 0.75f };

            Console.WriteLine($"  PhysVec   (world space):  {physVec}");
            Console.WriteLine($"  RenderVec (UV space):     {renderVec}");
        }

        // ── 3. Conflict resolution — fully qualified name ─────────────────
        // When two namespaces have a type with the same name and you can't alias,
        // use the fully qualified name: Namespace.TypeName.
        private static void ConflictResolutionDemo()
        {
            Console.WriteLine("\n--- 3. Conflict resolution — fully qualified name ---\n");

            // Both Physics and Rendering define Vector2.
            // Without aliases we'd write:
            var pv = new ConsoleApp1YTRecap.GameEngine.Physics.Vector2 { X = 10, Y = 20 };
            var rv = new ConsoleApp1YTRecap.GameEngine.Rendering.Vector2 { U = 0.1f, V = 0.9f };

            Console.WriteLine($"  Physics.Vector2   (fully qualified): {pv}");
            Console.WriteLine($"  Rendering.Vector2 (fully qualified): {rv}");
            Console.WriteLine("\n  Alias (PhysVec / RenderVec) is the cleaner solution.");
        }

        // ── 4. Convention guide ───────────────────────────────────────────
        private static void ConventionGuide()
        {
            Console.WriteLine("\n--- 4. Namespace Conventions & Rules ---\n");

            Console.WriteLine("  Naming convention:");
            Console.WriteLine("    CompanyName.ProductName.Feature");
            Console.WriteLine("    e.g. Microsoft.EntityFrameworkCore.Query");
            Console.WriteLine("         ConsoleApp1YTRecap.Intermediate");

            Console.WriteLine("\n  One namespace per folder (standard rule):");
            Console.WriteLine("    /Intermediate/  → namespace ConsoleApp1YTRecap.Intermediate");
            Console.WriteLine("    /Utilities/     → namespace ConsoleApp1YTRecap.Utilities");
            Console.WriteLine("    /GameEngine/Physics/ → ConsoleApp1YTRecap.GameEngine.Physics");

            Console.WriteLine("\n  File-scoped namespace (C# 10+ — removes one level of indent):");
            Console.WriteLine("    namespace MyApp.Utils;   // ← semicolon, no braces");
            Console.WriteLine("    // entire file is inside MyApp.Utils");

            Console.WriteLine("\n  global using (C# 10+ — put in one file, applies project-wide):");
            Console.WriteLine("    global using System.Collections.Generic;");
            Console.WriteLine("    global using MyApp.Shared;");

            Console.WriteLine("\n  Rules:");
            Console.WriteLine("    • Namespace does NOT need to match physical folder (but should)");
            Console.WriteLine("    • Types in same namespace see each other without 'using'");
            Console.WriteLine("    • 'using' is per-file (or global if prefixed with 'global')");
            Console.WriteLine("    • Deeply nested namespaces can be aliased for readability");
        }
    }
}
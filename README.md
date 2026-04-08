# CSharp repository basics to advanced
It's based on Code Monkey Basics to Advance Course on Youtube. 
You can find it here [C#BasicsToAdvance](https://www.youtube.com/watch?v=IFayQioG71A&list=PLzDRvYVwl53t2GGC4rV_AmH7vSvSqjVmz)

### Events — "notify me when something happens"

Events solve the communication problem. One object needs to tell other objects that something occurred, without knowing who's listening or how many listeners there are. The VideoEncoder from your EP.5 doesn't know or care whether one thing or ten things subscribed to its OnEncoded event. It just fires and forgets. The direction of knowledge is one-way: the publisher knows nothing about its subscribers.

### Interfaces — "I promise I can do these things"

Interfaces solve the capability problem. You have objects that are completely unrelated in their nature — a Hero, a Monster, a TreasureChest — but they all share a capability: they can be attacked. The interface is a contract that says "if you sign this, I guarantee you have these methods." The Warrior doesn't need to know what it's attacking, only that the target honours the contract.

### Generics — "write it once, work with any type"

Generics solve the reuse problem. You write an Inventory that manages slots, and you don't want to rewrite that same logic for weapons, then for potions, then for coins. The type is just a placeholder that gets filled in at compile time. Generics don't care about behaviour or communication — they only care about not repeating yourself while staying type-safe.

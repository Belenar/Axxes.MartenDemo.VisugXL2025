# Use Marten to build event sourced apps with no regrets (mostly)
This is the repo with slides and demo code for my Marten talk at Visug XL 2025.

## Abstract
Most applications are still being built on relational databases, and therefor focus on storing the current state of the system. In doing so, we usually sacrifice the history that led up to this state in the first place.

If we store the events instead of the resulting state, not only do we retain all of that history, but we also get a whole new programming paradigm at our disposal. We can rebuild state, do point-in-time replays of entities, re-create projected data, etc.

In this talk, I'll go over the core concepts of Event Sourcing and CQRS using code samples with the Marten Event Store in .NET. Not only will you get an idea bout how Event Sourcing works, but you'll see some of it in working code.

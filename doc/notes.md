# TODO

- manual testing
  - bugs (or features ;))
    - ThingyEater doesn't eat Thingies that stand in its way when it's seeking another Thingy.
    - ThingyEater should stop seeking the Thingy that is too far away (eg. when it's fallen out of bounds).
    - ThingyEater could change its seek target whenever a new one is placed and it's closer than the one being seeked
    - Sometimes (very rarely, hard to reproduce) ThingyEater starts seeking a thingy that is being carried 🙁
    -

- Go Far and Beyond
  - UI look & feel
  - particles
  - sfx
  - gfx

# Ubiquitous language

placeable cubes
placeable thingy?
spawn
carry, carried, carrying

# What could be done better

- search the solution for "// TODO:"

- data binding using eg. Peppermint DataBinding asset for items, collections, etc.

- separate models for item configs and view models for binding

- placement
  - projecting to the floor
  - off-ground
  - on-ground
  - translation constraint (XZ plane)

# Non-func requirements

  - Solution reliability, no bugs in scenarios described above
  - Architecture approach - don’t worry about over-engineering too much - we would like to see how you approach a real project. If you think you can make something better, but you don’t have time for it - tell us about it during the interview.
  - Code readability
  - Usage of Zenject or other Dependency Injection framework (not obligatory, but nice to have)
  - Unit tests will be a nice addition

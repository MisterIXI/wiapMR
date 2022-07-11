### Ownership lock
when tracking is lost while holding a cube input_up is never called. Which in turn never enables ownership transfer. The owner touching it again (and releasing it while tracked) returns it to the right state again.

## Alpha values on game piece not upodated on load
alpha value (transparency) only updated when changed in unity
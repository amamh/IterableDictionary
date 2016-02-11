### Purpose

This is a special data structure based on the Dictionary/Hash. It's a dictionary that provides a "cursor" with two methods:
- `MoveNext(): Boolean` => returns true if the cursor can move to the next pair
- `GetCurrent(): T` => reads current pair

The cursor behaviour will allow for the dictionary to change while guaranteeing that its users (readers) still get all values including the ones
that were changed. This is best explained with an example:

- Dictionary D = {1: 10, 2: 20, 3: 30}
- Get cursor C on D pointing to the first pair 1: 10
- C reads all of D
- D gets a pair updated, now it is {1: 11, 2: 20, 3: 30}
- C will now be able to move again i.e. `MoveNext` will return true and will move to the updated node 1: 11
- C reads the first node again 1: 11

For more examples, please check the unit tests.

### Summary

The result is a dictionary that can be updated in the middle of being read making sure that all readers regardless of how much they have
progressed will end up getting the correct up-to-date values. Please note that this doesn't take into account multithreading.

### todo
- Multithreading.
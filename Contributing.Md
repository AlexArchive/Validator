*Thank you very much for your interest in contributing.*

## What to contribute?

Although I _might_ take this project in a different direction in the future, for now, this project is a direct port of [validator.js](https://github.com/chriso/validator.js).

Choose a method from [validator.js](https://github.com/chriso/validator.js) that looks interesting to you (such as `isAscii`) and emulate it here.

## Code Guidelines

- Please keep each line of code under 80 characters wide.
- Please use the same conventions as me.
- Please write a unit test with 3 or more test cases for each method you contribute. (If you are not sure what to use for test data, refer to the validator.js [tests](https://github.com/chriso/validator.js/blob/master/test/validators.js) and search the name of the function you are porting.)
- Please put methods of considerable length (10+ lines) in their own `partial` class like I did for the [`IsBase64.cs`](https://github.com/ByteBlast/Validator/blob/master/Validator/IsBase64.cs) method.

## Pull Request Guidelines

- Please ensure that your commits are small.
- Please ensure that the code compiles and the tests pass after each commit. Why? Because then I can more easily pick and choose commits to merge.
- If you want to change existing code, please submit an issue describing the changes you want to make and *why* first. Thank you in advanced for that.
- Please do not concern yourself with performance too much. I am a firm believer that _preoptimization is the root of all evil_ - I would much rather readable code than minor optimizations.
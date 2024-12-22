## Input disassembly

Let's study what MONAD does with the inputs to try figure out what the input should be.

Monad has 14 chunks of 18 lines that look very similar (each chunk starts with the input instruction). From my input:

|#| Chunk 1    | Chunk 2   | Chunk 3   | Pseudocode           | Comments                      |
|--|:----------|:----------|:----------|:---------------------|:------------------------------|
|1 | `inp w`   | `inp w`   | `inp w`   | `w = input_digit`    | Get input digit               |
|2 | `mul x 0` | `mul x 0` | `mul x 0` | `x = x * 0`          | Reset x                       |
|3 | `add x z` | `add x z` | `add x z` | `x = x + z`          | Add z                         |
|4 | `mod x 26`| `mod x 26`| `mod x 26`| `x = x % 26`         | Mod 26                        |
|5 | `div z 1` | `div z 1` | `div z 1` | `z = z / 1`          | Keeps z value from last chunk |
|6 | `add x 11`| `add x 12`| `add x 10`| `x = x + 11`         |	<-- Only change in chunks     |
|7 | `eql x w` | `eql x w` | `eql x w` | `x = (x==w) ? 1 : 0` |                               |
|8 | `eql x 0` | `eql x 0` | `eql x 0` | `x = (x==0) ? 1 : 0` | x != w -> 1                   |
|9 | `mul y 0` | `mul y 0` | `mul y 0` | `y = y * 0`          | Reset y                       |
|10| `add y 25`| `add y 25`| `add y 25`| `y = y + 25`         | Add 25                        |
|11| `mul y x` | `mul y x` | `mul y x` | `y = y * x`          | Product by x (0 or 1)         |
|12| `add y 1` | `add y 1` | `add y 1` | `y++`                | Increment                     |
|13| `mul z y` | `mul z y` | `mul z y` | `z = z * y`          | **z is assigned**             |
|14| `mul y 0` | `mul y 0` | `mul y 0` | `y = y * 0`          | Reset y                       |
|15| `add y w` | `add y w` | `add y w` | `y = y + w`          | y = w                         |
|16| `add y 8` | `add y 8` | `add y 8` | `y = y + 8`          | y += 8                        |
|17| `mul y x` | `mul y x` | `mul y x` | `y = y * x`          | y *= x (0 or 1)               |
|18| `add z y` | `add z y` | `add z y` | `z = z + y`          | **z is assigned**             | 


I've pasted only the 3 first chunks above, along with pseudo code and comments. Notice two things:

- Line 6 is the only change each chunk has.
- Z is only assigned in three places (line 5 means that Z is the one from the last chunk)

Let's dissect a little bit deeper and put together some of the lines above:

```csharp
w = digit
x = (z % 26) + (chunk_value_that_is_different)
x = (x != w) ? 1 : 0;
y = (25 * x) + 1
z = z * y
y = (w + 8) * x
z = z + y
```

Ok this seems to be more manageable. So, some questions now before we move forward: 

1. **_Are all variables reset for each chunk?_**

The answer is **all of them but the Z** (which makes sense given the problem). 

- W is initialized to the digit. (line 1)
- X is initialized to 0 (line 2)
- Y is initialized to 0 (line 9)
 
 
2. _**How do the digits and the value that is different for each chunk come into play?**_

Basically, for each chunk we are testing the following:

```
x = (z % 26) + (chunk value)
if( x != digit)
   y = 26
else
  y = 1

z *=y

if( x != digit)
	y = (digit + 8)
else 
   y = 0

z = z + y
```

These 2 ifs are the ones that transform z. We have to find a digit for each chunk that complies with that condition. 

The code above can be seen as an operation betwen the digit and the chunk value. If the operation(digit) == chunk_value ; we've got a digit. 


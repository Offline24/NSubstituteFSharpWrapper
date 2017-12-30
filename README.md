# NSubstitute FSharp Wrapper

Travis master: [![Build Status](https://travis-ci.org/Offline24/NSubstituteFSharpWrapper.svg?branch=master)](https://travis-ci.org/Offline24/NSubstituteFSharpWrapper) develop: [![Build Status](https://travis-ci.org/Offline24/NSubstituteFSharpWrapper.svg?branch=develop)](https://travis-ci.org/Offline24/NSubstituteFSharpWrapper)

Provides simple F# syntax for NSubstitute.

## Usage examples

Check unit tests to see all possible uses: [MockTests.fs](NSubstituteFSharpWrapper.Tests/MockTests.fs)

### Mock create

```fsharp
open NSubstituteFSharpWrapper

let mock = mock<MyInterface> ()
```

### Mocking field value

```fsharp
mock |> arrange (fun x -> x.IntField) |> returns 1
```

### Mocking function return value

```fsharp
mock |> arrange (fun x -> x.AddFunction any any) |> returns 1
```

```fsharp
mock |> arrange (fun x -> x.AddFunction (is 1) (is 2)) |> returns 3
mock |> arrange (fun x -> x.AddFunction any any) |> returns 5
```

### Checking function calls

```fsharp
mock |> receivedAnyTimes (fun x -> x.AddFunction (is 1) (is 2))
```

```fsharp
mock |> receivedTimes 4 |> (fun x -> x.AddFunction any any)
//OR
mock |> received 4 times |> (fun x -> x.AddFunction any any)
```

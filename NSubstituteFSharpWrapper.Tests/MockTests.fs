module MockTests

open NSubstituteFSharpWrapper
open NSubstitute.Exceptions
open Xunit
open FsUnit.Xunit

type MyInterface =
   abstract member Add: int -> int -> int
   abstract member AddAndStringify: int -> seq<int> -> string
   abstract member IntField: int
   abstract member StringField: string

[<Fact>]
let ``fields return default value`` () =
    let mock = mock<MyInterface> ()
    mock.Object.IntField |> should equal 0
    mock.Object.StringField |> should equal ""

[<Fact>]
let ``functions return default value`` () =
    let mock = mock<MyInterface> ()
    mock.Object.Add 1 2 |> should equal 0
    mock.Object.AddAndStringify 1 [2] |> should equal ""

[<Fact>]
let ``field arrange -> returns`` () =
    let mock = mock<MyInterface> ()
    mock |> arrange (fun x -> x.IntField) |> returns 1
    mock.Object.IntField |> should equal 1

[<Fact>]
let ``simple function arrange -> returns with any arguments`` () =
    let mock = mock<MyInterface> ()
    mock |> arrange (fun x -> x.Add any any) |> returns 1
    mock.Object.Add 1 2 |> should equal 1

[<Fact>]
let ``complex function arrange -> returns with any arguments`` () =
    let mock = mock<MyInterface> ()
    mock |> arrange (fun x -> x.AddAndStringify any any) |> returns "ok"
    mock.Object.AddAndStringify 1 [2] |> should equal "ok"

[<Fact>]
let ``simple function arrange -> returns with specified arguments`` () =
    let mock = mock<MyInterface> ()
    mock |> arrange (fun x -> x.Add (is 1) (is 2)) |> returns 3
    mock |> arrange (fun x -> x.Add (is 4) (is 5)) |> returns 6
    mock.Object.Add 1 2 |> should equal 3
    mock.Object.Add 4 5 |> should equal 6

[<Fact>]
let ``received any call with any arguments, positive path`` () =
    let mock = mock<MyInterface> ()
    mock.Object.Add 1 2 |> ignore
    mock |> receivedAnyTimes (fun x -> x.Add any any)

[<Fact>]
let ``received any call with any arguments, nagative path`` () =
    let mock = mock<MyInterface> ()
    let shouldThrow = fun () -> (mock |> receivedAnyTimes (fun x -> x.Add any any)) |> ignore
    shouldThrow |> should throw typeof<ReceivedCallsException>

[<Fact>]
let ``received any call with specified arguments, positive path`` () =
    let mock = mock<MyInterface> ()
    mock.Object.Add 1 2 |> ignore
    mock |> receivedAnyTimes (fun x -> x.Add (is 1) (is 2))

[<Fact>]
let ``received any call with specified arguments, nagative path`` () =
    let mock = mock<MyInterface> ()
    let shouldThrow = fun () -> (mock |> receivedAnyTimes (fun x -> x.Add (is 1) (is 2))) |> ignore
    shouldThrow |> should throw typeof<ReceivedCallsException>

[<Fact>]
let ``received specified number of calls with any arguments, positive path`` () =
    let mock = mock<MyInterface> ()
    mock.Object.Add 1 2 |> ignore
    mock.Object.Add 2 3 |> ignore
    mock |> receivedTimes 2 |> (fun x -> x.Add any any)

[<Fact>]
let ``received specified number of calls with any arguments, negative path`` () =
    let mock = mock<MyInterface> ()
    mock.Object.Add 1 2 |> ignore
    let shouldThrow = fun () -> (mock |> receivedTimes 2 |> (fun x -> x.Add any any)) |> ignore
    shouldThrow |> should throw typeof<ReceivedCallsException>
    
[<Fact>]
let ``received specified number of calls with specified arguments, positive path`` () =
    let mock = mock<MyInterface> ()
    mock.Object.Add 1 2 |> ignore
    mock.Object.Add 2 3 |> ignore
    mock.Object.Add 1 2 |> ignore
    mock |> receivedTimes 2 |> (fun x -> x.Add (is 1) (is 2))

[<Fact>]
let ``received specified number of calls with specified arguments, negative path`` () =
    let mock = mock<MyInterface> ()
    mock.Object.Add 1 2 |> ignore
    mock.Object.Add 2 3 |> ignore
    mock.Object.Add 1 2 |> ignore
    let shouldThrow = fun () -> (mock |> receivedTimes 2 |> (fun x -> x.Add (is 2) (is 3))) |> ignore
    shouldThrow |> should throw typeof<ReceivedCallsException>
    
[<Fact>]
let ``received specified number of calls with any arguments, so fluent, positive path`` () =
    let mock = mock<MyInterface> ()
    mock.Object.Add 1 2 |> ignore
    mock.Object.Add 2 3 |> ignore
    mock |> received 2 times |> (fun x -> x.Add any any)

[<Fact>]
let ``received specified number of calls with any arguments, so fluent, negative path`` () =
    let mock = mock<MyInterface> ()
    mock.Object.Add 1 2 |> ignore
    let shouldThrow = fun () -> (mock |> received 2 times |> (fun x -> x.Add any any)) |> ignore
    shouldThrow |> should throw typeof<ReceivedCallsException>
module MockTests

open NSubstitute.Exceptions
open NSubstitute
open NSubstituteFSharpWrapper
open Xunit
open FsUnit.Xunit
open System.Net.Http
open System.Threading.Tasks

type TestModel = { 
    IntValue: int
    StringValue: string
    }

type TestInterface =
    abstract member SimpleFunction: int -> int -> int
    abstract member ComplexFunction: int -> seq<int> -> string
    abstract member TestModelFunction: TestModel -> unit
    abstract member CSharpStyleAsyncMethod: unit -> Task<string>
    abstract member IntField: int
    abstract member StringField: string

type TestCalculatorType () =
    abstract add: int -> int -> int
    default this.add a b = a + b
    abstract zero: int
    default this.zero = 0

type ``mock create tests`` () =
    [<Fact>]
    let ``create mock`` () =
        let mock = mock<TestInterface> ()
        mock.Object |> should not' (be null)

    [<Fact>]
    let ``use mock`` () =
        let mock = mock<TestInterface> ()
        let testFunction (arg : TestInterface) = arg.IntField
        mock.Object |> testFunction |> ignore

 type ``returning values tests`` () =
    [<Fact>]
    let ``int field returns default value when not configured`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<TestInterface> ()
        let nsubstituteMockResult = nsubstituteMock.IntField

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<TestInterface> ()
        let wrapperMockResult = wrapperMock.Object.IntField

        // Should equal
        wrapperMockResult |> should equal nsubstituteMockResult

    [<Fact>]
    let ``string field returns default value when not configured`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<TestInterface> ()
        let nsubstituteMockResult = nsubstituteMock.StringField

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<TestInterface> ()
        let wrapperMockResult = wrapperMock.Object.StringField

        // Should equal
        wrapperMockResult |> should equal nsubstituteMockResult

    [<Fact>]
    let ``simple function returns default value when not configured`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<TestInterface> ()
        let nsubstituteMockResult = nsubstituteMock.SimpleFunction 1 2

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<TestInterface> ()
        let wrapperMockResult = wrapperMock.Object.SimpleFunction 1 2

        // Should equal
        wrapperMockResult |> should equal nsubstituteMockResult

    [<Fact>]
    let ``complex function returns default value when not configured`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<TestInterface> ()
        let nsubstituteMockResult = nsubstituteMock.ComplexFunction 1 [1;2]

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<TestInterface> ()
        let wrapperMockResult = wrapperMock.Object.ComplexFunction 1 [1;2]

        // Should equal
        wrapperMockResult |> should equal nsubstituteMockResult

    [<Fact>]
    let ``int field returns configured values`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<TestInterface> ()
        nsubstituteMock.IntField.Returns 1 |> ignore
        let nsubstituteMockResult = nsubstituteMock.IntField

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<TestInterface> ()
        wrapperMock |> setup (fun x -> x.IntField) |> returns 1
        let wrapperMockResult = wrapperMock.Object.IntField

        // Should equal
        wrapperMockResult |> should equal nsubstituteMockResult

    [<Fact>]
    let ``string field returns configured values`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<TestInterface> ()
        nsubstituteMock.StringField.Returns "ok" |> ignore
        let nsubstituteMockResult = nsubstituteMock.StringField

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<TestInterface> ()
        wrapperMock |> setup (fun x -> x.StringField) |> returns "ok"
        let wrapperMockResult = wrapperMock.Object.StringField

        // Should equal
        wrapperMockResult |> should equal nsubstituteMockResult

    [<Fact>]
    let ``simple function returns configured value with any arguments`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<TestInterface> ()
        (nsubstituteMock.SimpleFunction (Arg.Any ()) (Arg.Any ())).Returns 1 |> ignore
        let nsubstituteMockResult = nsubstituteMock.SimpleFunction 1 2

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<TestInterface> ()
        wrapperMock |> setup (fun x -> x.SimpleFunction arg.any arg.any) |> returns 1
        let wrapperMockResult = wrapperMock.Object.SimpleFunction 1 2

        // Should equal
        wrapperMockResult |> should equal nsubstituteMockResult

    [<Fact>]
    let ``complex function returns configured value with any arguments`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<TestInterface> ()
        (nsubstituteMock.ComplexFunction (Arg.Any ()) (Arg.Any ())).Returns "ok" |> ignore
        let nsubstituteMockResult = nsubstituteMock.ComplexFunction 1 [1;2]

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<TestInterface> ()
        wrapperMock |> setup (fun x -> x.ComplexFunction arg.any arg.any) |> returns "ok"
        let wrapperMockResult = wrapperMock.Object.ComplexFunction 1 [1;2]

        // Should equal
        wrapperMockResult |> should equal nsubstituteMockResult

    [<Fact>]
    let ``simple function returns configured value with specified arguments`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<TestInterface> ()
        (nsubstituteMock.SimpleFunction (Arg.Is 1) (Arg.Is 2)).Returns 3 |> ignore
        (nsubstituteMock.SimpleFunction (Arg.Is 4) (Arg.Is 5)).Returns 6 |> ignore
        let nsubstituteMockResult_1 = nsubstituteMock.SimpleFunction 1 2
        let nsubstituteMockResult_2 = nsubstituteMock.SimpleFunction 4 5

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<TestInterface> ()
        wrapperMock |> setup (fun x -> x.SimpleFunction (arg.is 1) (arg.is 2)) |> returns 3
        wrapperMock |> setup (fun x -> x.SimpleFunction (arg.is 4) (arg.is 5)) |> returns 6
        let wrapperMockResult_1 = wrapperMock.Object.SimpleFunction 1 2
        let wrapperMockResult_2 = wrapperMock.Object.SimpleFunction 4 5

        // Should equal
        wrapperMockResult_1 |> should equal nsubstituteMockResult_1
        wrapperMockResult_2 |> should equal nsubstituteMockResult_2

    [<Fact>]
    let ``complex function returns configured value with specified arguments`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<TestInterface> ()
        (nsubstituteMock.ComplexFunction (Arg.Is 1) (Arg.Is [1;2])).Returns "1 1;2" |> ignore
        (nsubstituteMock.ComplexFunction (Arg.Is 3) (Arg.Is [3;4])).Returns "3 3;4" |> ignore
        let nsubstituteMockResult_1 = nsubstituteMock.ComplexFunction 1 [1;2]
        let nsubstituteMockResult_2 = nsubstituteMock.ComplexFunction 3 [3;4]

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<TestInterface> ()
        wrapperMock |> setup (fun x -> x.ComplexFunction (arg.is 1) (arg.is [1;2])) |> returns "1 1;2"
        wrapperMock |> setup (fun x -> x.ComplexFunction (arg.is 3) (arg.is [3;4])) |> returns "3 3;4"
        let wrapperMockResult_1 = wrapperMock.Object.ComplexFunction 1 [1;2]
        let wrapperMockResult_2 = wrapperMock.Object.ComplexFunction 3 [3;4]

        // Should equal
        wrapperMockResult_1 |> should equal nsubstituteMockResult_1
        wrapperMockResult_2 |> should equal nsubstituteMockResult_2

    [<Fact>]
    let ``same configuration overwriting`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<TestCalculatorType> ()
        nsubstituteMock.zero.Returns 10 |> ignore
        nsubstituteMock.zero.Returns 20 |> ignore
        let nsubstituteMockResult = nsubstituteMock.zero

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<TestCalculatorType> ()
        wrapperMock |> setup (fun x -> x.zero) |> returns 10
        wrapperMock |> setup (fun x -> x.zero) |> returns 20
        let wrapperMockResult = wrapperMock.Object.zero

        // Should equal
        wrapperMockResult |> should equal nsubstituteMockResult

    [<Fact>]
    let ``more general configurations overwriting`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<TestCalculatorType> ()
        (nsubstituteMock.add (Arg.Any ()) (Arg.Any ())).Returns 1 |> ignore
        (nsubstituteMock.add (Arg.Any ()) (Arg.Is 1)).Returns 2 |> ignore
        (nsubstituteMock.add (Arg.Is 1) (Arg.Any ())).Returns 3 |> ignore
        (nsubstituteMock.add (Arg.Is 1) (Arg.Is 1)).Returns 4 |> ignore
        let nsubstituteMockResult_1 = nsubstituteMock.add 9 9
        let nsubstituteMockResult_2 = nsubstituteMock.add 9 1
        let nsubstituteMockResult_3 = nsubstituteMock.add 1 9
        let nsubstituteMockResult_4 = nsubstituteMock.add 1 1

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<TestCalculatorType> ()
        wrapperMock |> setup (fun x -> x.add arg.any arg.any) |> returns 1
        wrapperMock |> setup (fun x -> x.add arg.any (arg.is 1)) |> returns 2
        wrapperMock |> setup (fun x -> x.add (arg.is 1) arg.any) |> returns 3
        wrapperMock |> setup (fun x -> x.add (arg.is 1) (arg.is 1)) |> returns 4
        let wrapperMockResult_1 = wrapperMock.Object.add 9 9
        let wrapperMockResult_2 = wrapperMock.Object.add 9 1
        let wrapperMockResult_3 = wrapperMock.Object.add 1 9
        let wrapperMockResult_4 = wrapperMock.Object.add 1 1

        // Should equal
        wrapperMockResult_1 |> should equal nsubstituteMockResult_1
        wrapperMockResult_2 |> should equal nsubstituteMockResult_2
        wrapperMockResult_3 |> should equal nsubstituteMockResult_3
        wrapperMockResult_4 |> should equal nsubstituteMockResult_4

    [<Fact>]
    let ``WARNING: mocking multiple arguments F# function not work as expected`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<int -> int -> int> ()
        (nsubstituteMock (Arg.Any ()) (Arg.Any ())).Returns 1 |> ignore
        let nsubstituteMockResult = nsubstituteMock 1 1

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<int -> int -> int> ()
        wrapperMock |> setup (fun x -> x arg.any arg.any) |> returns 1
        let wrapperMockResult = wrapperMock.Object 1 1

        // Should equal
        wrapperMockResult |> should equal nsubstituteMockResult

        // NOT WORK AS EXPECTED
        let is = should
        nsubstituteMockResult |> is equal 0
        wrapperMockResult |> is equal 0

    [<Fact>]
    let ``mocking one argument F# function`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<string -> int> ()
        (nsubstituteMock (Arg.Any ())).Returns 1 |> ignore
        (nsubstituteMock (Arg.Is "ok")).Returns 2 |> ignore
        let nsubstituteMockResult_1 = nsubstituteMock "not ok"
        let nsubstituteMockResult_2 = nsubstituteMock "ok"

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<string -> int> ()
        wrapperMock |> setup (fun x -> x arg.any) |> returns 1
        wrapperMock |> setup (fun x -> x (arg.is "ok")) |> returns 2
        let wrapperMockResult_1 = wrapperMock.Object "not ok"
        let wrapperMockResult_2 = wrapperMock.Object "ok"

        // Should equal
        nsubstituteMockResult_1 |> should equal wrapperMockResult_1
        nsubstituteMockResult_2 |> should equal wrapperMockResult_2

        // Works as expected
        let is = should
        wrapperMockResult_1 |> is equal 1
        wrapperMockResult_2 |> is equal 2

    [<Fact>]
    let ``mocking zero argument F# function`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<unit -> int> ()
        nsubstituteMock().Returns 1 |> ignore
        let nsubstituteMockResult = nsubstituteMock ()

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<unit -> int> ()
        wrapperMock |> setup (fun x -> x ()) |> returns 1
        let wrapperMockResult = wrapperMock.Object ()

        // Should equal
        nsubstituteMockResult |> should equal wrapperMockResult

        // Works as expected
        let is = should
        wrapperMockResult |> is equal 1

    [<Fact>]
    let ``mocking async C# method`` () =
        // NSubstitute
        let nsubstituteMock = Substitute.For<TestInterface> ()
        nsubstituteMock.CSharpStyleAsyncMethod().Returns "ok" |> ignore
        let nsubstituteMockResult = nsubstituteMock.CSharpStyleAsyncMethod().Result

        // NSubstituteFSharpWrapper
        let wrapperMock = mock<TestInterface> ()
        wrapperMock |> setup (fun x -> x.CSharpStyleAsyncMethod ()) |> returnsAsync "ok"
        let wrapperMockResult = wrapperMock.Object.CSharpStyleAsyncMethod().Result

        // Should equal
        nsubstituteMockResult |> should equal wrapperMockResult

type ``checking received calls tests`` () =
    let rand = System.Random()
    let randInt () = rand.Next ()

    [<Fact>]
    let ``received one call with any arguments, positive path`` () =
        // Given
        let mock = mock<TestCalculatorType> ()
        
        // When
        mock.Object.add 1 2 |> ignore

        // Then
        mock |> check (fun x -> x.add arg.any arg.any) |> wasCalledOnce

    [<Fact>]
    let ``received one call with any arguments, nagative path - too few calls`` () =
        // Given
        let mock = mock<TestCalculatorType> ()

        // When

        // Then
        let shouldThrow = fun () -> (mock |> check (fun x -> x.add arg.any arg.any) |> wasCalledOnce)
        shouldThrow |> should throw typeof<ReceivedCallsException>

    [<Fact>]
    let ``received one call with any arguments, nagative path - too many calls`` () =
        // Given
        let mock = mock<TestCalculatorType> ()

        // When
        mock.Object.add 1 2 |> ignore
        mock.Object.add 2 2 |> ignore

        // Then
        let shouldThrow = fun () -> (mock |> check (fun x -> x.add arg.any arg.any) |> wasCalledOnce)
        shouldThrow |> should throw typeof<ReceivedCallsException>

    [<Fact>]
    let ``received two calls with any arguments, positive path`` () =
        // Given
        let mock = mock<TestCalculatorType> ()
        
        // When
        mock.Object.add 1 2 |> ignore
        mock.Object.add 2 2 |> ignore

        // Then
        mock |> check (fun x -> x.add arg.any arg.any) |> wasCalledTwice

    [<Fact>]
    let ``received two calls with any arguments, nagative path - too few calls`` () =
        // Given
        let mock = mock<TestCalculatorType> ()

        // When
        mock.Object.add 2 2 |> ignore

        // Then
        let shouldThrow = fun () -> (mock |> check (fun x -> x.add arg.any arg.any) |> wasCalledTwice)
        shouldThrow |> should throw typeof<ReceivedCallsException>

    [<Fact>]
    let ``received two calls with any arguments, nagative path - too many calls`` () =
        // Given
        let mock = mock<TestCalculatorType> ()

        // When
        mock.Object.add 2 2 |> ignore
        mock.Object.add 5 3 |> ignore
        mock.Object.add 1 5 |> ignore

        // Then
        let shouldThrow = fun () -> (mock |> check (fun x -> x.add arg.any arg.any) |> wasCalledTwice)
        shouldThrow |> should throw typeof<ReceivedCallsException>

    [<Fact>]
    let ``received at least one call with any arguments, positive path`` () =
        // Given
        let mock = mock<TestCalculatorType> ()

        // When
        for _ in [1..100] do
            mock.Object.add (randInt ()) (randInt ()) |> ignore

        // Then
        mock |> check (fun x -> x.add arg.any arg.any) |> wasCalledAtLeastOnce

    [<Fact>]
    let ``received at least one call with any arguments, nagative path - no calls`` () =
        // Given
        let mock = mock<TestCalculatorType> ()

        // When

        // Then
        let shouldThrow = fun () -> (mock |> check (fun x -> x.add arg.any arg.any) |> wasCalledAtLeastOnce)
        shouldThrow |> should throw typeof<ReceivedCallsException>

    [<Fact>]
    let ``received many calls with any arguments, positive path`` () =
        // Given
        let mock = mock<TestCalculatorType> ()

        // When
        for _ in [1..100] do
            mock.Object.add (randInt ()) (randInt ()) |> ignore

        // Then
        mock |> check (fun x -> x.add arg.any arg.any) |> wasCalled 100 times

    [<Fact>]
    let ``received many calls with any arguments, nagative path - too few calls`` () =
        // Given
        let mock = mock<TestCalculatorType> ()

        // When
        for _ in [1..100] do
            mock.Object.add (randInt ()) (randInt ()) |> ignore

        // Then
        let shouldThrow = fun () -> (mock |> check (fun x -> x.add arg.any arg.any) |> wasCalled 101 times)
        shouldThrow |> should throw typeof<ReceivedCallsException>

    [<Fact>]
    let ``received many calls with any arguments, nagative path - too many calls`` () =
        // Given
        let mock = mock<TestCalculatorType> ()

        // When
        for _ in [1..100] do
            mock.Object.add (randInt ()) (randInt ()) |> ignore

        // Then
        let shouldThrow = fun () -> (mock |> check (fun x -> x.add arg.any arg.any) |> wasCalled 99 times)
        shouldThrow |> should throw typeof<ReceivedCallsException>

    [<Fact>]
    let ``received at least one call with specified arguments, positive path`` () =
        // Given
        let mock = mock<TestCalculatorType> ()

        // When
        mock.Object.add 1 2 |> ignore

        // Then
        mock |> check (fun x -> x.add (arg.is 1) (arg.is 2)) |> wasCalledAtLeastOnce

    [<Fact>]
    let ``received at least one call with specified arguments, negative path`` () =
        // Given
        let mock = mock<TestCalculatorType> ()

        // When
        mock.Object.add 1 2 |> ignore

        // Then
        let shouldThrow = fun () -> (mock |> check (fun x -> x.add (arg.is 1) (arg.is 3)) |> wasCalledAtLeastOnce)
        shouldThrow |> should throw typeof<ReceivedCallsException>

    [<Fact>]
    let ``received two calls with specified arguments, positive path`` () =
        // Given
        let mock = mock<TestCalculatorType> ()

        // When
        mock.Object.add 1 2 |> ignore
        mock.Object.add 1 3 |> ignore
        mock.Object.add 2 2 |> ignore

        // Then
        mock |> check (fun x -> x.add arg.any (arg.is 2)) |> wasCalledTwice

    [<Fact>]
    let ``received two calls with specified arguments, negative path`` () =
        // Given
        let mock = mock<TestCalculatorType> ()

        // When
        mock.Object.add 1 2 |> ignore
        mock.Object.add 2 3 |> ignore
        mock.Object.add 2 2 |> ignore

        // Then
        let shouldThrow = fun () -> (mock |>  check (fun x -> x.add (arg.is 1) arg.any) |> wasCalledTwice)
        shouldThrow |> should throw typeof<ReceivedCallsException>
    
    [<Fact>]
    let ``received two calls with matched simple arguments, positive path`` () =
        // Given
        let mock = mock<TestCalculatorType> ()

        // When
        mock.Object.add 1 0 |> ignore
        mock.Object.add 1 1 |> ignore
        mock.Object.add 2 2 |> ignore

        // Then
        mock |> check (fun mock -> mock.add arg.any (arg.match' (fun arg -> arg >= 1))) |> wasCalledTwice

    [<Fact>]
    let ``received two calls with matched simple arguments, negative path`` () =
        // Given
        let mock = mock<TestCalculatorType> ()

        // When
        mock.Object.add 1 0 |> ignore
        mock.Object.add 1 1 |> ignore

        // Then
        let shouldThrow = fun () -> (mock |> check (fun mock -> mock.add arg.any (arg.match' (fun arg -> arg >= 1))) |> wasCalledTwice)
        shouldThrow |> should throw typeof<ReceivedCallsException>

    [<Fact>]
    let ``received calls with matched complex arguments, positive path`` () =
        // Given
        let testFunc i s (testInterface : TestInterface) = testInterface.TestModelFunction ({IntValue = i; StringValue = s})
        let mock = mock<TestInterface> ()

        // When
        mock.Object |> testFunc 0 "start"
        mock.Object |> testFunc 1 "wait"
        mock.Object |> testFunc 1 "wait"
        mock.Object |> testFunc 2 "end"

        // Then
        mock 
        |> check (fun mock -> mock.TestModelFunction (arg.match' (fun arg -> arg.IntValue = 0 && arg.StringValue = "start"))) 
        |> wasCalledOnce

        mock 
        |> check (fun mock -> mock.TestModelFunction (arg.match' (fun arg -> arg.StringValue = "wait"))) 
        |> wasCalledAtLeastOnce

        mock 
        |> check (fun mock -> mock.TestModelFunction (arg.match' (fun arg -> arg.IntValue = 2 && arg.StringValue = "end"))) 
        |> wasCalledOnce

    [<Fact>]
    let ``received calls with matched complex arguments, negative path`` () =
        // Given
        let testFunc i s (testInterface : TestInterface) = testInterface.TestModelFunction ({IntValue = i; StringValue = s})
        let mock = mock<TestInterface> ()

        // When
        mock.Object |> testFunc 0 "" |> ignore
        mock.Object |> testFunc 0 "end" |> ignore

        // Then
        let shouldThrow_1 = fun () -> ( 
                                        mock 
                                        |> check (fun mock -> mock.TestModelFunction (arg.match' (fun arg -> arg.IntValue = 0 && arg.StringValue = "start"))) 
                                        |> wasCalledOnce
                                    )
        shouldThrow_1 |> should throw typeof<ReceivedCallsException>

        let shouldThrow_2 = fun () -> ( 
                                        mock 
                                        |> check (fun mock -> mock.TestModelFunction (arg.match' (fun arg -> arg.StringValue = "wait"))) 
                                        |> wasCalledAtLeastOnce
                                    )
        shouldThrow_2 |> should throw typeof<ReceivedCallsException>

        let shouldThrow_3 = fun () -> ( 
                                        mock 
                                        |> check (fun mock -> mock.TestModelFunction (arg.match' (fun arg -> arg.IntValue = 2 && arg.StringValue = "end"))) 
                                        |> wasCalledOnce
                                    )
        shouldThrow_3 |> should throw typeof<ReceivedCallsException>

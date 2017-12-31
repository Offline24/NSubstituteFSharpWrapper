module public NSubstituteFSharpWrapper

    open NSubstitute

    module public Internals =
        type public mockSetup<'R> internal (object : 'R) =
            member this.Object = object
        type public mockCheck<'T, 'R when 'T : not struct> internal (targetToCheck : 'T -> 'R, object : 'T) = 
            member this.Object = object
            member this.TargetToCheck = targetToCheck
    
    open Internals
    open System
    open System.Threading.Tasks
    open System.Linq.Expressions

    module public Keywords =
        type public times internal () = class end

    [<AutoOpen>]
    module public GlobalKeywords =
        let public times = Keywords.times ()

    type public mock<'T when 'T : not struct> () =
        let mockInstance = Substitute.For<'T> ()
        member this.Object = mockInstance

    [<AutoOpen>]
    module public Argument =
        type public arg<'T> = 
            static member any = Arg.Any<'T> ()
            static member is (value : 'T) = Arg.Is (value = value)
            static member match' (predicate : Expression<Predicate<'T>>) = Arg.Is (predicate = predicate)

        [<Obsolete("Use arg.any instead.")>]
        let public any<'T> = arg<'T>.any
        [<Obsolete("Use arg.is instead.")>]
        let public is<'T> (value : 'T) = arg<'T>.is value

    [<AutoOpen>]
    module public Setup =
        let setup<'T, 'R when 'T : not struct> (targetToSetup : ('T -> 'R)) (mock' : mock<'T>) = mock'.Object |> targetToSetup |> mockSetup<'R>
        [<Obsolete("Use setup instead.")>]
        let arrange<'T, 'R when 'T : not struct> (targetToSetup : ('T -> 'R)) (mock' : mock<'T>) = setup targetToSetup mock'
        let returns<'T, 'R when 'T : not struct> (value : 'R) (fromSetup : mockSetup<'R>) = SubstituteExtensions.Returns (fromSetup.Object, value) |> ignore
        let returnsAsync<'T, 'R when 'T : not struct> (value : 'R) (fromSetup : mockSetup<Task<'R>>) = fromSetup |> returns (Task.Factory.StartNew<'R> (fun () -> value))

    [<AutoOpen>]
    module public Check =
        let check<'T, 'R when 'T : not struct> (targetToCheck : 'T -> 'R) (mock' : mock<'T>) = mockCheck (targetToCheck, mock'.Object)
        let wasCalledAtLeastOnce<'T, 'R when 'T : not struct> (fromCheck : mockCheck<'T, 'R>) =
            SubstituteExtensions.Received fromCheck.Object
            |> fromCheck.TargetToCheck
            |> ignore
        let wasCalled<'T, 'R when 'T : not struct> (requiredNumberOfCalls : int) (times : Keywords.times) (fromCheck : mockCheck<'T, 'R>) =
            SubstituteExtensions.Received (fromCheck.Object, requiredNumberOfCalls)
            |> fromCheck.TargetToCheck
            |> ignore
        let wasCalledOnce<'T, 'R when 'T : not struct> (fromCheck : mockCheck<'T, 'R>) = fromCheck |> wasCalled 1 times
        let wasCalledTwice<'T, 'R when 'T : not struct> (fromCheck : mockCheck<'T, 'R>) = fromCheck |>  wasCalled 2 times
        let wasNotCalled<'T, 'R when 'T : not struct> (fromCheck : mockCheck<'T, 'R>) =
            SubstituteExtensions.DidNotReceive fromCheck.Object
            |> fromCheck.TargetToCheck
            |> ignore

        [<Obsolete("Use check and wasCalledAnyNumberOfTimes instead.")>]
        let receivedAnyTimes<'T, 'R when 'T : not struct> (targetToCheck : 'T -> 'R) (mock' : mock<'T>) = SubstituteExtensions.Received (mock'.Object) |> targetToCheck |> ignore
        [<Obsolete("Use check and wasCalled instead.")>]
        let receivedTimes<'T when 'T : not struct> (i : int) (m : mock<'T>) = SubstituteExtensions.Received (m.Object, i)
        [<Obsolete("Use check and wasCalled instead.")>]
        let received<'T when 'T : not struct> (requiredNumberOfCalls : int) (times : Keywords.times) (mock' : mock<'T>) = receivedTimes requiredNumberOfCalls mock'


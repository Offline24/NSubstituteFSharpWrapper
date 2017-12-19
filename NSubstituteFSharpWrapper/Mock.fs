module NSubstituteFSharpWrapper

    open NSubstitute

    type mock<'T when 'T : not struct> () =
        let mockInstancce = Substitute.For<'T> ()
        member this.Object = mockInstancce

    type mockResult<'R> (object : 'R) =
        member this.Object = object

    let any<'T> = Arg.Any<'T> ()
    let is<'T> (v:'T) = Arg.Is v

    let arrange<'T, 'R when 'T : not struct> (f : ('T -> 'R)) (m : mock<'T>) = m.Object |> f |> mockResult<'R>
    let returns<'T, 'R when 'T : not struct> (v : 'R) (r : mockResult<'R>) = SubstituteExtensions.Returns (r.Object, v) |> ignore

    let receivedAnyTimes<'T, 'R when 'T : not struct> (f : ('T -> 'R)) (m : mock<'T>) = SubstituteExtensions.Received (m.Object) |> f |> ignore
    let receivedTimes<'T when 'T : not struct> (i : int) (m : mock<'T>) = SubstituteExtensions.Received (m.Object, i)
    let received<'T when 'T : not struct> (i : int) u (m : mock<'T>) = receivedTimes i m
    let times = ()
     

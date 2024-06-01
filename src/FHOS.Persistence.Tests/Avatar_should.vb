Public Class Avatar_should
    <Fact>
    Sub have_default_values_upon_instantiation()
        Dim sut = CreateAvatar()
        sut.Actor.ShouldBeNull
        Should.Throw(Of InvalidOperationException)(Sub() sut.Pop())
        Should.Throw(Of NullReferenceException)(Sub() sut.Push(Nothing))
    End Sub
    <Fact>
    Sub push_pop_actor()
        Dim universe = CreateUniverse()
        Dim sut = CreateAvatar(universe)
        Dim actor = CreateActor("map name", "map type", 3, 2, "location type", 1, 1, "actor type", "actor name", universe:=universe)
        sut.Push(actor)
        sut.Actor.Id.ShouldBe(actor.Id)
        sut.Pop()
        sut.Actor.ShouldBeNull
    End Sub
End Class

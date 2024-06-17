Public Class Universe_should
    <Fact>
    Sub have_default_values_upon_instantiation()
        Dim sut As IUniverse = CreateUniverse()
        sut.ShouldNotBeNull
        sut.Factory.ShouldNotBeNull
        sut.Avatar.ShouldNotBeNull
        sut.Messages.ShouldNotBeNull
        sut.Groups.ShouldBeEmpty
        sut.Actors.ShouldBeEmpty
        Should.Throw(Of InvalidOperationException)(Function() sut.Turn)
    End Sub
    <Fact>
    Sub set_and_retrieve_turn()
        Dim sut As IUniverse = CreateUniverse()
        Const Turn = 1234
        sut.Turn = Turn
        sut.Turn.ShouldBe(Turn)
    End Sub
End Class



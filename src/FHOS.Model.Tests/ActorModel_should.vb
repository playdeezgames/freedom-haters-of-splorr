Public Class ActorModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        Should.Throw(Of ArgumentNullException)(Function() sut.Sprite)
        sut.Name.ShouldBe("actor name")
    End Sub

    Private Function CreateSut() As IActorModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Board.GetLocation((0, 0)).Actor
    End Function
End Class

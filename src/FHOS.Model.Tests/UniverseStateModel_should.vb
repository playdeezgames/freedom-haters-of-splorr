Public Class UniverseStateModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Turn.ShouldBe(1)
        sut.Board.ShouldNotBeNull
        sut.Avatar.ShouldNotBeNull
        sut.Messages.ShouldNotBeNull
    End Sub

    Private Function CreateSut() As IUniverseStateModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State
    End Function
End Class

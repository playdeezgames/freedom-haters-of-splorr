Public Class UniverseStateModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Turn.ShouldBe(1)
        sut.Avatar.ShouldNotBeNull
        sut.Messages.ShouldNotBeNull
        sut.GetLocation((0, 0)).ShouldNotBeNull
    End Sub

    Private Function CreateSut() As IUniverseStateModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State
    End Function
End Class

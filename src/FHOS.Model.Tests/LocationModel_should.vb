Public Class LocationModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Exists.ShouldBeTrue
        sut.LocationType.ShouldNotBeNull
        sut.Actor.ShouldNotBeNull
        sut.HasDetails.ShouldBeTrue
    End Sub

    Private Function CreateSut() As ILocationModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Board.GetLocation((0, 0))
    End Function
End Class

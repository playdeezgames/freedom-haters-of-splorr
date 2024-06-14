Public Class LocationModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Exists.ShouldBeTrue
        sut.LocationType.ShouldNotBeNull
        sut.Actor.ShouldNotBeNull
        sut.HasActor.ShouldBeTrue
    End Sub
    <Fact>
    Sub have_default_non_existent_values_upon_initialization()
        Dim sut = CreateSut(-1, -1)
        sut.Exists.ShouldBeFalse
        sut.LocationType.ShouldBeNull
        sut.Actor.ShouldBeNull
        sut.HasActor.ShouldBeFalse
    End Sub

    Private Function CreateSut(Optional column As Integer = 0, Optional row As Integer = 0) As ILocationModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.GetLocation((column, row))
    End Function
End Class

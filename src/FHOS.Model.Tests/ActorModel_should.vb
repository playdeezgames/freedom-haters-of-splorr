Public Class ActorModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        Should.Throw(Of ArgumentNullException)(Function() sut.Sprite)
        sut.Name.ShouldBe("actor name")
        Should.Throw(Of InvalidOperationException)(Function() sut.Group)
        Should.Throw(Of KeyNotFoundException)(Function() sut.Subtype)
        Should.Throw(Of KeyNotFoundException)(Function() sut.IsStarSystem)
        Should.Throw(Of KeyNotFoundException)(Function() sut.IsPlanet)
        Should.Throw(Of KeyNotFoundException)(Function() sut.IsPlanetVicinity)
        Should.Throw(Of KeyNotFoundException)(Function() sut.IsSatellite)
        sut.Position.X.ShouldBe(0)
        sut.Position.Y.ShouldBe(0)
        Should.Throw(Of NullReferenceException)(Function() sut.PlanetCount)
        Should.Throw(Of NullReferenceException)(Function() sut.SatelliteCount)
        Should.Throw(Of NullReferenceException)(Function() sut.StarSystem)
        Should.Throw(Of NullReferenceException)(Function() sut.Faction)
        Should.Throw(Of NullReferenceException)(Function() sut.PlanetVicinity)
    End Sub

    Private Function CreateSut() As IActorModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Board.GetLocation((0, 0)).Actor
    End Function
End Class

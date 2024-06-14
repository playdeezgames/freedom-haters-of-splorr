Public Class GroupModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Name.ShouldBe("group name")
        Should.Throw(Of InvalidOperationException)(Function() sut.Authority)
        Should.Throw(Of InvalidOperationException)(Function() sut.Standards)
        Should.Throw(Of InvalidOperationException)(Function() sut.Conviction)
        Should.Throw(Of InvalidOperationException)(Function() sut.PlanetCount)
        Should.Throw(Of InvalidOperationException)(Function() sut.SatelliteCount)
        Should.Throw(Of InvalidOperationException)(Function() sut.Position)
        sut.PlanetList.ShouldBeEmpty
        sut.SatelliteList.ShouldBeEmpty
        sut.FactionsPresent.ShouldBeEmpty
        sut.StarSystem.ShouldBeNull
        sut.Planet.ShouldBeNull
        sut.Faction.ShouldBeNull
        sut.StarTypeName.ShouldBeNull
        Should.Throw(Of InvalidOperationException)(Function() sut.RelationNameTo(sut))
    End Sub

    Private Function CreateSut() As IGroupModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Bio.Group
    End Function
End Class

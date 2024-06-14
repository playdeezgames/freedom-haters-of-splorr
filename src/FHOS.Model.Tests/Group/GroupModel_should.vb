Public Class GroupModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Name.ShouldBe("group name")

        Should.Throw(Of InvalidOperationException)(Function() sut.Standards)
        Should.Throw(Of InvalidOperationException)(Function() sut.Conviction)

        sut.ChildPlanets.ShouldBeEmpty
        sut.ChildSatellites.ShouldBeEmpty
        sut.ChildPlanetFactions.ShouldBeEmpty
        sut.ParentStarSystem.ShouldBeNull
        sut.ParentPlanet.ShouldBeNull
        sut.ParentFaction.ShouldBeNull
        Should.Throw(Of InvalidOperationException)(Function() sut.RelationNameTo(sut))
    End Sub

    Private Function CreateSut() As IGroupModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Bio.Group
    End Function
End Class

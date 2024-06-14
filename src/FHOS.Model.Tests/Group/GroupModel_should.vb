Public Class GroupModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Name.ShouldBe("group name")
        Should.Throw(Of InvalidOperationException)(Function() sut.RelationNameTo(sut))

        sut.ChildPlanets.ShouldBeEmpty
        sut.ChildSatellites.ShouldBeEmpty
        sut.ChildPlanetFactions.ShouldBeEmpty

        sut.Parents.ShouldNotBeNull
        sut.ParentStarSystem.ShouldBeNull
        sut.ParentPlanet.ShouldBeNull
        sut.ParentFaction.ShouldBeNull
    End Sub

    Private Function CreateSut() As IGroupModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Bio.Group
    End Function
End Class

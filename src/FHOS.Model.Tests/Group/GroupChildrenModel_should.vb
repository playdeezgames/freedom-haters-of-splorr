Public Class GroupChildrenModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.ChildPlanetFactions.ShouldBeEmpty
        sut.ChildPlanets.ShouldBeEmpty
        sut.ChildSatellites.ShouldBeEmpty
    End Sub

    Private Function CreateSut() As IGroupChildrenModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Bio.Faction.Children
    End Function

End Class

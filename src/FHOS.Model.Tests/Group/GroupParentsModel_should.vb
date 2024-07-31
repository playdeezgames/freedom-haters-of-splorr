Public Class GroupParentsModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.StarSystem.ShouldBeNull
        sut.Planet.ShouldBeNull
        sut.Faction.ShouldBeNull
    End Sub

    Private Function CreateSut() As IGroupParentsModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Bio.Faction.Parents
    End Function

End Class

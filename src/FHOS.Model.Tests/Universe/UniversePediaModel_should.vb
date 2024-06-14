Public Class UniversePediaModel_should
    <Fact>
    Sub have_initial_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Factions.ShouldBeEmpty
        sut.StarSystems.ShouldBeEmpty
        sut.PlanetVicinities.ShouldBeEmpty
        sut.Satellites.ShouldBeEmpty
    End Sub

    Private Function CreateSut() As IUniversePediaModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).Pedia
    End Function
End Class

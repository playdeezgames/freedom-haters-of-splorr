Public Class UniversePediaModel_should
    <Fact>
    Sub have_initial_values_upon_initialization()
        Dim sut = CreateSut()
        sut.FactionList.ShouldBeEmpty
        sut.StarSystemList.ShouldBeEmpty
        sut.PlanetVicinityList.ShouldBeEmpty
        sut.SatelliteList.ShouldBeEmpty
    End Sub

    Private Function CreateSut() As IUniversePediaModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).Pedia
    End Function
End Class

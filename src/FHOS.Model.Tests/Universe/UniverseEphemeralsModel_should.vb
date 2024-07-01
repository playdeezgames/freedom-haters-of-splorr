Public Class UniverseEphemeralsModel_should
    <Fact>
    Sub have_initial_values_upon_initialization()
        Dim sut = CreateSut()
        ValidateDefaultEphemerals(sut)
    End Sub

    Private Shared Sub ValidateDefaultEphemerals(sut As IUniverseEphemeralsModel)
        sut.SelectedFaction.ShouldBeEmpty
        sut.SelectedPlanet.ShouldBeEmpty
        sut.SelectedSatellite.ShouldBeEmpty
        sut.SelectedStarSystem.ShouldBeEmpty
        sut.CurrentOffer.ShouldBeNull
        sut.CurrentPrice.ShouldBeNull
    End Sub


    Private Function CreateSut() As IUniverseEphemeralsModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).Ephemerals
    End Function

End Class

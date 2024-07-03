Public Class UniverseEphemeralsModel_should
    <Fact>
    Sub have_initial_values_upon_initialization()
        Dim sut = CreateSut()
        ValidateDefaultEphemerals(sut)
    End Sub

    <Fact>
    Sub set_sell_quantity()
        Dim sut = CreateSut()
        Const givenQuantity = 10
        sut.SellQuantity = givenQuantity
        sut.SellQuantity.ShouldBe(givenQuantity)
    End Sub

    Private Shared Sub ValidateDefaultEphemerals(sut As IUniverseEphemeralsModel)
        sut.SelectedFaction.ShouldBeEmpty
        sut.SelectedPlanet.ShouldBeEmpty
        sut.SelectedSatellite.ShouldBeEmpty
        sut.SelectedStarSystem.ShouldBeEmpty
        sut.CurrentOffer.ShouldBeNull
        sut.CurrentPrice.ShouldBeNull
        sut.SellQuantity.ShouldBe(0)
        sut.InventoryItemStack.ShouldBeNull
    End Sub


    Private Function CreateSut() As IUniverseEphemeralsModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).Ephemerals
    End Function

End Class

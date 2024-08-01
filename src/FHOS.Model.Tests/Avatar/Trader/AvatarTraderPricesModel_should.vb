Public Class AvatarTraderPricesModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Name.ShouldBe("Scrap")
        sut.UnitPrice.ShouldBe(0)
        sut.CanBuy.ShouldBeFalse
        sut.MaximumQuantity.ShouldBe(0)
        sut.InventoryQuantity.ShouldBe(1)
    End Sub
    Private Shared Function CreateSut() As IAvatarTraderPriceModel
        Return CreateOneStepUniverse(AddressOf BuildTraderUniverse).State.Avatar.Yokes.Trader.Prices.Single
    End Function

End Class

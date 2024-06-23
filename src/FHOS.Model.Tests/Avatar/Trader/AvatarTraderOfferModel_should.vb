Public Class AvatarTraderOfferModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Name.ShouldBe("Scrap(x1)")
        sut.Quantity.ShouldBe(1)
    End Sub
    <Fact>
    Sub sell_an_item()
        Dim avatar = CreateOneStepUniverse(AddressOf BuildTraderUniverse).State.Avatar
        avatar.Inventory.Summary.ShouldHaveSingleItem
        Dim sut = avatar.Trader.Offers.Single
        Should.NotThrow(Sub() sut.Sell(1))
        avatar.Inventory.Summary.ShouldBeEmpty
        avatar.Jools.ShouldBe(1)
    End Sub
    Private Shared Function CreateSut() As IAvatarTraderOfferModel
        Return CreateOneStepUniverse(AddressOf BuildTraderUniverse).State.Avatar.Trader.Offers.Single
    End Function
End Class

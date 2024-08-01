Public Class AvatarTraderOfferModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.NameAndQuantity.ShouldBe("Scrap(x1)")
        sut.Name.ShouldBe("Scrap")
        sut.Quantity.ShouldBe(1)
    End Sub
    <Fact>
    Sub sell_an_item()
        Dim avatar = CreateOneStepUniverse(AddressOf BuildTraderUniverse).State.Avatar
        avatar.Inventory.ItemStacks.ShouldHaveSingleItem
        Dim sut = avatar.Yokes.Trader.Offers.Single
        sut.JoolsOffered(1).ShouldBe(1)
        Should.NotThrow(Sub() sut.Sell(1))
        avatar.Inventory.ItemStacks.ShouldBeEmpty
        avatar.Jools.ShouldBe(1)
    End Sub
    Private Shared Function CreateSut() As IAvatarTraderOfferModel
        Return CreateOneStepUniverse(AddressOf BuildTraderUniverse).State.Avatar.Yokes.Trader.Offers.Single
    End Function
End Class

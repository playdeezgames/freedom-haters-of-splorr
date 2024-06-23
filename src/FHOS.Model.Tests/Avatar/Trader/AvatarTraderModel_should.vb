Public Class AvatarTraderModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.IsActive.ShouldBeTrue
        sut.HasOffers.ShouldBeTrue
        sut.Offers.ShouldHaveSingleItem
        sut.HasPrices.ShouldBeFalse
        sut.Prices.ShouldBeEmpty
        sut.Trader.ShouldNotBeNull
        Should.NotThrow(Sub() sut.Leave())
    End Sub

    Private Shared Function CreateSut() As IAvatarTraderModel
        Return CreateOneStepUniverse(AddressOf BuildTraderUniverse).State.Avatar.Trader
    End Function
End Class

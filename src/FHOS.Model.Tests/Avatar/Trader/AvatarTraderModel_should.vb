Public Class AvatarTraderModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.IsActive.ShouldBeFalse
        sut.HasOffers.ShouldBeFalse
        sut.Offers.ShouldBeEmpty
        sut.HasPrices.ShouldBeFalse
        sut.Prices.ShouldBeEmpty
        sut.Trader.ShouldBeNull
        Should.NotThrow(Sub() sut.Leave())
    End Sub

    Private Shared Function CreateSut() As IAvatarTraderModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Trader
    End Function
End Class

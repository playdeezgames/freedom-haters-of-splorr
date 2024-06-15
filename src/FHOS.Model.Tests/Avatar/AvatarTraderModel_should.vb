Public Class AvatarTraderModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.IsActive.ShouldBeFalse
    End Sub

    Private Shared Function CreateSut() As IAvatarTraderModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Trader
    End Function
End Class

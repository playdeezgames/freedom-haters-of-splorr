Public Class AvatarWalletModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        Should.Throw(Of NullReferenceException)(Function() sut.Jools)
    End Sub

    Private Function CreateSut() As IAvatarWalletModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Wallet
    End Function
End Class

Public Class AvatarModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Bio.ShouldNotBeNull
        sut.Interaction.ShouldNotBeNull
        sut.Stack.ShouldNotBeNull
        sut.StarGate.ShouldNotBeNull
        sut.State.ShouldNotBeNull
        sut.Status.ShouldNotBeNull
        sut.Verbs.ShouldNotBeNull
        sut.Vessel.ShouldNotBeNull
        sut.Wallet.ShouldNotBeNull
    End Sub

    Private Shared Function CreateSut() As IAvatarModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar
    End Function
End Class

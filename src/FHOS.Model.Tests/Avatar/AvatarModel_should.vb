Public Class AvatarModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Bio.ShouldNotBeNull
        sut.Interaction.ShouldNotBeNull
        sut.Stack.ShouldNotBeNull
        sut.StarGate.ShouldNotBeNull
        sut.Trader.ShouldNotBeNull
        sut.State.ShouldNotBeNull
        sut.Status.ShouldNotBeNull
        sut.Verbs.ShouldNotBeNull
        sut.Vessel.ShouldNotBeNull
        sut.Inventory.ShouldNotBeNull
        Should.Throw(Of NullReferenceException)(Function() sut.Jools)
    End Sub

    Private Shared Function CreateSut() As IAvatarModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar
    End Function
End Class

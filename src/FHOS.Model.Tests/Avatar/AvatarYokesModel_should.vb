Public Class AvatarYokesModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Interaction.ShouldNotBeNull
        sut.Shipyard.ShouldNotBeNull
        sut.Trader.ShouldNotBeNull
    End Sub

    Private Shared Function CreateSut() As IAvatarYokesModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Yokes
    End Function
End Class

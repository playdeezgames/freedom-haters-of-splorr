Public Class AvatarShipyardModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.IsActive.ShouldBeTrue
        sut.Shipyard.ShouldNotBeNull
        Should.NotThrow(Sub() sut.Leave())
    End Sub

    Private Shared Function CreateSut() As IAvatarShipyardModel
        Return CreateOneStepUniverse(AddressOf BuildShipyardUniverse).State.Avatar.Shipyard
    End Function

End Class

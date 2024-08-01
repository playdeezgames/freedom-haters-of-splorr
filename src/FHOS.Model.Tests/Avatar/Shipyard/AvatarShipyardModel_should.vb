Public Class AvatarShipyardModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.IsActive.ShouldBeTrue
        sut.Specimen.ShouldNotBeNull
        Should.NotThrow(Sub() sut.Leave())
        sut.CanInstallEquipment.ShouldBeFalse
        sut.InstallableEquipmentSlots.ShouldBeEmpty
        sut.CanChangeEquipment.ShouldBeTrue
        sut.ChangeableEquipmentSlots.ShouldHaveSingleItem
        sut.CanUninstallEquipment.ShouldBeFalse
        sut.UninstallableEquipmentSlots.ShouldBeEmpty
    End Sub

    Private Shared Function CreateSut() As IAvatarShipyardModel
        Return CreateOneStepUniverse(AddressOf BuildShipyardUniverse).State.Avatar.Yokes.Shipyard
    End Function

End Class

Public Class AvatarEquipmentSlotModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Item.DisplayName.ShouldBe("StarLume Fuel Mark I")
        sut.Item.InstallFee.ShouldBe(10)
        sut.SlotName.ShouldBe("Fuel Supply")
        sut.InstallableItems.ShouldHaveSingleItem
        sut.HasItem.ShouldBeTrue
        Should.Throw(Of ArgumentNullException)(Sub() sut.Equip(Nothing))
        Should.Throw(Of InvalidOperationException)(Sub() sut.Equip(sut.InstallableItems.Single))
        Dim uninstalledItem = sut.Unequip
        uninstalledItem.ShouldNotBeNull
        sut.UninstallFee.ShouldBe(0)
        Should.NotThrow(Sub() sut.Equip(uninstalledItem))
        sut.UninstallFee.ShouldBe(5)
    End Sub

    Private Function CreateSut() As IAvatarEquipmentSlotModel
        Return CreateOneStepUniverse(AddressOf BuildShipyardUniverse).State.Avatar.Equipment.Slots.First
    End Function
End Class

Public Class ActorEquipmentSlotModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.SlotName.ShouldBe("Fuel Supply")
        sut.InstallableItems.ShouldHaveSingleItem
        Should.Throw(Of NotImplementedException)(Sub() sut.Equip(Nothing))
    End Sub
    <Fact>
    Sub uninstall_item_from_slot()
        Dim avatar = CreateOneStepUniverse(AddressOf BuildShipyardUniverse).State.Avatar
        Dim sut = CreateSut(avatar)
        Dim oldItem = sut.Unequip()
        oldItem.ShouldNotBeNull
        oldItem.DisplayName.ShouldBe("StarLume Fuel Mark I")
        avatar.Equipment.Slots.ShouldBeEmpty
        avatar.Inventory.ItemStacks.Single(Function(x) x.ItemName = "StarLume Fuel Mark I").Count.ShouldBe(1)
    End Sub

    Private Function CreateSut(Optional avatar As IAvatarModel = Nothing) As IActorEquipmentSlotModel
        Return If(avatar, CreateOneStepUniverse(AddressOf BuildShipyardUniverse).State.Avatar).Shipyard.ChangeableEquipmentSlots.First
    End Function
End Class

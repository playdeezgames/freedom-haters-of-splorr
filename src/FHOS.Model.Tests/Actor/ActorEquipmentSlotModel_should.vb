Public Class ActorEquipmentSlotModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.SlotName.ShouldBe("Fuel Supply")
        sut.InstallableItems.ShouldHaveSingleItem
        Should.Throw(Of ArgumentNullException)(Sub() sut.Equip(Nothing))
        sut.UninstallFee.ShouldBe(5)
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
    <Fact>
    Sub install_item_to_slot_after_uninstalling()
        Dim avatar = CreateOneStepUniverse(AddressOf BuildShipyardUniverse).State.Avatar
        Dim sut = CreateSut(avatar)
        sut.Unequip()
        Const ItemName = "StarLume Fuel Mark II"
        Dim item = avatar.Inventory.ItemStacks.Single(Function(x) x.ItemName = ItemName).Items.Single
        sut.Equip(item)
        avatar.Equipment.Slots.Where(Function(x) x.Item.DisplayName = ItemName).ShouldHaveSingleItem
        avatar.Inventory.ItemStacks.SingleOrDefault(Function(x) x.ItemName = ItemName).ShouldBeNull
    End Sub
    <Fact>
    Sub fail_to_install_item_to_slot_before_uninstalling_old_item()
        Dim avatar = CreateOneStepUniverse(AddressOf BuildShipyardUniverse).State.Avatar
        Dim sut = CreateSut(avatar)
        Const ItemName = "StarLume Fuel Mark II"
        Dim item = avatar.Inventory.ItemStacks.Single(Function(x) x.ItemName = ItemName).Items.Single
        Should.Throw(Of InvalidOperationException)(Sub() sut.Equip(item))
    End Sub

    Private Function CreateSut(Optional avatar As IAvatarModel = Nothing) As IActorEquipmentSlotModel
        Return If(avatar, CreateOneStepUniverse(AddressOf BuildShipyardUniverse).State.Avatar).Shipyard.ChangeableEquipmentSlots.First
    End Function
End Class

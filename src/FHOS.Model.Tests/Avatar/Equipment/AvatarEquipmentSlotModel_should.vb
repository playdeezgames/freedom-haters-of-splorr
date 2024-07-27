Public Class AvatarEquipmentSlotModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Item.DisplayName.ShouldBe("StarLume Fuel Mark I")
        sut.Item.InstallFee.ShouldBe(10)
        sut.SlotName.ShouldBe("Fuel Supply")
    End Sub

    Private Function CreateSut() As IAvatarEquipmentSlotModel
        Return CreateOneStepUniverse(AddressOf BuildShipyardUniverse).State.Avatar.Equipment.Slots.First
    End Function
End Class

Public Class ActorEquipmentSlotModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.SlotName.ShouldBe("Fuel Supply")
    End Sub

    Private Function CreateSut() As IActorEquipmentSlotModel
        Return CreateOneStepUniverse(AddressOf BuildShipyardUniverse).State.Avatar.Shipyard.ChangeableEquipmentSlots.First
    End Function
End Class

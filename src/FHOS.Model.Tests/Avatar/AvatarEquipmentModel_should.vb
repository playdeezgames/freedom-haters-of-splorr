Public Class AvatarEquipmentModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Slots.ShouldBeEmpty
    End Sub

    Private Function CreateSut() As IAvatarEquipmentModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Equipment
    End Function
End Class

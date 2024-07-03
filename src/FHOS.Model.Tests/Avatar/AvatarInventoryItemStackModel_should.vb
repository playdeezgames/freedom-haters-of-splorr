Public Class AvatarInventoryItemStackModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.ShouldBeNull
    End Sub

    Private Function CreateSut() As IAvatarInventoryItemStackModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Inventory.ItemStacks.FirstOrDefault
    End Function
End Class

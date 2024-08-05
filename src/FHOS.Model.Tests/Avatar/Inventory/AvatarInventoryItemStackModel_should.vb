Public Class AvatarInventoryItemStackModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.ShouldNotBeNull
        sut.ItemTypeName.ShouldBe("Scrap")
        sut.Count.ShouldBe(1)
        sut.Items.ShouldHaveSingleItem
        sut.Substacks.ShouldHaveSingleItem
    End Sub

    Private Function CreateSut() As IAvatarInventoryItemStackModel
        Return CreateOneStepUniverse(AddressOf BuildOneInventoryItemUniverse).State.Avatar.Inventory.ItemStacks.FirstOrDefault
    End Function
End Class

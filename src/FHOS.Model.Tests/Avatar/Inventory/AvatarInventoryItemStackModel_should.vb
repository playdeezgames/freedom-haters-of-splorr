Public Class AvatarInventoryItemStackModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.ShouldNotBeNull
        sut.ItemTypeName.ShouldBe("Scrap")
        sut.Count.ShouldBe(1)
        sut.Description.ShouldNotBeNull
        sut.CanUse.ShouldBeFalse
        sut.Items.ShouldHaveSingleItem
        sut.Substacks.ShouldHaveSingleItem
        Should.NotThrow(Sub() sut.Use())
    End Sub

    Private Function CreateSut() As IAvatarInventoryItemStackModel
        Return CreateOneStepUniverse(AddressOf BuildOneInventoryItemUniverse).State.Avatar.Inventory.ItemStacks.FirstOrDefault
    End Function
End Class

Public Class AvatarInventoryItemSubstackModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.ShouldNotBeNull
        sut.EntityName.ShouldBe("Scrap")
    End Sub

    Private Function CreateSut() As IAvatarInventoryItemSubstackModel
        Return CreateOneStepUniverse(AddressOf BuildOneInventoryItemUniverse).State.Avatar.Inventory.ItemStacks.FirstOrDefault.Substacks.FirstOrDefault
    End Function

End Class

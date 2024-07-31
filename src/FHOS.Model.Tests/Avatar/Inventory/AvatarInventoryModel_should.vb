Public Class AvatarInventoryModel_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.ItemStacks.ShouldBeEmpty
    End Sub

    Private Function CreateSut() As IAvatarInventoryModel
        Return CreateOneStepUniverse(AddressOf BuildLonelyUniverse).State.Avatar.Inventory
    End Function
End Class

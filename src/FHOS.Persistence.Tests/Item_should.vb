Public Class Item_should
    <Fact>
    Sub have_an_id()
        Dim sut As IItem = CreateSut()
        sut.Id.ShouldBe(0)
    End Sub

    Private Shared Function CreateSut(
                                     Optional itemType As String = "item type") As IItem
        Return CreateItem(itemType)
    End Function

End Class

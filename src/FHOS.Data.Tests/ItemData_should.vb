Public Class ItemData_should
    Inherits EntityData_should(Of IItemData)
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Statistics.ShouldBeEmpty
        sut.Metadatas.ShouldBeEmpty
    End Sub
    Protected Overrides Function CreateSut() As IItemData
        Return New ItemData
    End Function
End Class


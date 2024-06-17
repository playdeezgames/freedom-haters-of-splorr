Public Class StoreData_should
    Inherits EntityData_should(Of IStoreData)
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Metadatas.ShouldBeEmpty
    End Sub
    Protected Overrides Function CreateSut() As IStoreData
        Return New StoreData
    End Function
End Class

Public Class StoreData_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut As IStoreData = New StoreData
        sut.Flags.ShouldBeEmpty
        sut.Statistics.ShouldBeEmpty
        sut.Metadatas.ShouldBeEmpty
    End Sub
End Class

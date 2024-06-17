Public Class GroupData_should
    Inherits EntityData_should(Of IGroupData)
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Metadatas.ShouldBeEmpty
        sut.Children.ShouldBeEmpty
        sut.Parents.ShouldBeEmpty
    End Sub
    Protected Overrides Function CreateSut() As IGroupData
        Return New GroupData
    End Function
End Class

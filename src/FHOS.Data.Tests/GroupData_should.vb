Public Class GroupData_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut As IGroupData = New GroupData
        sut.Flags.ShouldBeEmpty
        sut.Statistics.ShouldBeEmpty
        sut.Metadatas.ShouldBeEmpty
        sut.Children.ShouldBeEmpty
        sut.Parents.ShouldBeEmpty
    End Sub

End Class

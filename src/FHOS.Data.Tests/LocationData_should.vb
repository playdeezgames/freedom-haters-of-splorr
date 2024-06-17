Public Class LocationData_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut As ILocationData = New LocationData
        sut.Flags.ShouldBeEmpty
        sut.Statistics.ShouldBeEmpty
        sut.Metadatas.ShouldBeEmpty
    End Sub
End Class

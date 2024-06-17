Public Class LocationData_should
    Inherits EntityData_should(Of ILocationData)

    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Metadatas.ShouldBeEmpty
    End Sub

    Protected Overrides Function CreateSut() As ILocationData
        Return New LocationData
    End Function
End Class

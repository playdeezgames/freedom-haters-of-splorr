Public Class MapData_should
    Inherits EntityData_should(Of IMapData)
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Metadatas.ShouldBeEmpty
        sut.Locations.ShouldBeEmpty
        sut.YokedGroups.ShouldBeEmpty
    End Sub
    Protected Overrides Function CreateSut() As IMapData
        Return New MapData
    End Function
End Class

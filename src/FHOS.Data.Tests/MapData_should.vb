Public Class MapData_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut As IMapData = New MapData
        sut.Flags.ShouldBeEmpty
        sut.Statistics.ShouldBeEmpty
        sut.Metadatas.ShouldBeEmpty
        sut.Locations.ShouldBeEmpty
        sut.YokedGroups.ShouldBeEmpty
    End Sub

End Class

Public Class MapData_should
    Inherits EntityData_should(Of IMapData)
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Locations.ShouldBeEmpty
        sut.YokedGroups.ShouldBeEmpty
    End Sub
    <Fact>
    Sub default_given_location_to_nothing()
        Dim sut = CreateSut()
        Should.Throw(Of KeyNotFoundException)(Sub() sut.GetLocation(1))
    End Sub
    <Fact>
    Sub set_location()
        Dim sut = CreateSut()
        Const locationId = 1
        Const index = 10
        sut.SetLocation(index, locationId)
        sut.GetLocation(index).ShouldBe(locationId)
    End Sub
    Protected Overrides Function CreateSut() As IMapData
        Return New MapData
    End Function
End Class

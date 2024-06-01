Public Class Location_should
    <Fact>
    Sub have_an_id()
        Dim sut As ILocation = CreateSut()
        sut.Id.ShouldBe(0)
    End Sub

    Private Function CreateSut(
                              Optional mapName As String = "map name",
                              Optional mapType As String = "map type",
                              Optional mapColumns As Integer = 3,
                              Optional mapRows As Integer = 4,
                              Optional locationType As String = "location type",
                              Optional column As Integer = 0,
                              Optional row As Integer = 0,
                              Optional universe As IUniverse = Nothing,
                              Optional data As UniverseData = Nothing) As ILocation

        Return CreateMap(mapName, mapType, mapColumns, mapRows, locationType, universe, data).GetLocation(column, row)
    End Function

    <Fact>
    Sub have_default_values_upon_instantiation()
        Const column = 1
        Const row = 1
        Const locationType = "location type"
        Dim sut As ILocation = CreateSut(column:=column, row:=row, locationType:=locationType)
        sut.Map.ShouldNotBeNull
        sut.Column.ShouldBe(column)
        sut.Row.ShouldBe(row)
        sut.EntityType.ShouldBe(locationType)
        sut.Actor.ShouldBeNull
        sut.Tutorial.ShouldBeNull
        sut.TargetLocation.ShouldBeNull
        sut.HasTargetLocation.ShouldBeFalse
    End Sub
End Class

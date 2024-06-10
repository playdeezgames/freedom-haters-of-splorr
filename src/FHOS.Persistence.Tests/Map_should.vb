Public Class Map_should
    Private Const SutColumns = 2
    Private Const SutRows = 3
    Private Shared Function CreateSut(Optional universe As IUniverse = Nothing) As IMap
        Const mapName = "map name"
        Const mapType = "map type"
        Const locationType = "location type"
        Return CreateMap(
            mapName,
            mapType,
            SutColumns,
            SutRows,
            locationType,
            universe:=universe)
    End Function
    <Fact>
    Sub set_name()
        Const entityName = "entity name"
        Dim sut = CreateSut()
        sut.EntityName = entityName
        sut.EntityName.ShouldBe(entityName)
    End Sub
    <Fact>
    Sub have_an_id()
        Dim sut As IMap = CreateSut()
        sut.Id.ShouldBe(0)
    End Sub
    <Fact>
    Sub get_locations_by_column_row()
        Dim sut As IMap = CreateSut()
        For Each column In Enumerable.Range(0, SutColumns)
            For Each row In Enumerable.Range(0, SutRows)
                Dim location = sut.GetLocation(column, row)
                location.ShouldNotBeNull
                location.Position.Column.ShouldBe(column)
                location.Position.Row.ShouldBe(row)
            Next
        Next
    End Sub
    <Theory>
    <InlineData(-1, -1)>
    <InlineData(-1, 0)>
    <InlineData(0, -1)>
    <InlineData(SutColumns, 0)>
    <InlineData(0, SutRows)>
    <InlineData(SutColumns, SutRows)>
    Sub yield_no_result_for_locations_out_of_bounds(column As Integer, row As Integer)
        Dim sut As IMap = CreateSut()
        sut.GetLocation(column, row).ShouldBeNull
    End Sub
    <Fact>
    Sub have_no_yoked_groups()
        Const yokeType = "yoke type"
        Dim sut As IMap = CreateSut()
        sut.YokedGroup(yokeType).Shouldbenull
    End Sub
    <Fact>
    Sub yoke_groups()
        Const yokeType = "yoke type"
        Dim universe = CreateUniverse()
        Dim group = CreateGroup(universe:=universe)
        Dim sut As IMap = CreateSut(universe:=universe)
        sut.YokedGroup(yokeType) = group
        sut.YokedGroup(yokeType).Id.ShouldBe(group.Id)
    End Sub
    <Fact>
    Sub unyoke_groups()
        Const yokeType = "yoke type"
        Dim universe = CreateUniverse()
        Dim group = CreateGroup(universe:=universe)
        Dim sut As IMap = CreateSut(universe:=universe)
        sut.YokedGroup(yokeType) = group
        sut.YokedGroup(yokeType) = Nothing
        sut.YokedGroup(yokeType).ShouldBeNull
    End Sub
End Class

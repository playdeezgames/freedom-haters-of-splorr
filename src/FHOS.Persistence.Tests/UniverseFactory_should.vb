Public Class UniverseFactory_should
    <Fact>
    Sub create_group()
        Dim universe = CreateUniverse()
        Dim sut As IUniverseFactory = CreateUniverseFactory(universe)
        Const groupName = "group name"
        Const groupType = "group type"
        Dim group = sut.CreateGroup(groupType, groupName)
        group.ShouldNotBeNull
        group.GroupType.ShouldBe(groupType)
        group.Name.ShouldBe(groupName)
        universe.Groups.Count.ShouldBe(1)
        group.LegacyGroup.ShouldBeNull
        Should.Throw(Of InvalidOperationException)(Function() group.MinimumPlanetCount)
        group.PlanetCount.ShouldBe(0)
        group.SatelliteCount.ShouldBe(0)
        Should.Throw(Of InvalidOperationException)(Function() group.Authority)
        Should.Throw(Of InvalidOperationException)(Function() group.Standards)
        Should.Throw(Of InvalidOperationException)(Function() group.Conviction)
        group.Parents.ShouldBeEmpty
        group.Children.ShouldBeEmpty
        Should.Throw(Of NullReferenceException)(Sub() group.AddParent(Nothing))
        Should.NotThrow(Sub() group.RemoveParent(Nothing))
    End Sub
    <Fact>
    Sub create_map()
        Dim sut As IUniverseFactory = CreateUniverseFactory()
        Const mapName = "map name"
        Const mapType = "map type"
        Const mapColumns = 2
        Const mapRows = 3
        Const defaultLocationType = "default location type"
        Dim map = sut.CreateMap(mapName, mapType, mapColumns, mapRows, defaultLocationType)
        map.MapType.ShouldBe(mapType)
        map.Name.ShouldBe(mapName)
        map.Locations.Count.ShouldBe(mapColumns * mapRows)
        map.Locations.All(Function(x) x.LocationType = defaultLocationType).ShouldBeTrue
        map.Size.Columns.ShouldBe(mapColumns)
        map.Size.Rows.ShouldBe(mapRows)
        map.Id.ShouldBe(0)
    End Sub
    <Fact>
    Sub create_store()
        Dim sut As IUniverseFactory = CreateUniverseFactory()
        Const currentValue = 100
        Dim store = sut.CreateStore(currentValue)
        store.CurrentValue.ShouldBe(currentValue)
        store.MaximumValue.ShouldBeNull
        store.MinimumValue.ShouldBeNull
        store.Percent.ShouldBeNull
        store.Id.ShouldBe(0)
    End Sub
    <Fact>
    Sub create_item()
        Dim sut As IUniverseFactory = CreateUniverseFactory()
        Const itemType = "item type"
        Dim item = sut.CreateItem(itemType)
        item.ShouldNotBeNull
        item.Id.ShouldBe(0)
    End Sub
End Class

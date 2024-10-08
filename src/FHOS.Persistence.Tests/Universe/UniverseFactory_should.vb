﻿Public Class UniverseFactory_should
    <Fact>
    Sub create_group()
        Dim universe = CreateUniverse()
        Dim sut As IUniverseFactory = CreateUniverseFactory(universe)
        Const groupName = "group name"
        Const groupType = "group type"
        Dim group = sut.CreateGroup(groupType, groupName)
        group.ShouldNotBeNull
        group.EntityType.ShouldBe(groupType)
        group.EntityName.ShouldBe(groupName)
        universe.Groups.Count.ShouldBe(1)
        group.Parents.ShouldBeEmpty
        group.Children.ShouldBeEmpty
        group.Values.ShouldBeEmpty
        Should.Throw(Of NullReferenceException)(Sub() group.AddParent(Nothing))
        Should.NotThrow(Sub() group.RemoveParent(Nothing))
        Should.NotThrow(Sub() group.AddValue(1))
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
        map.EntityType.ShouldBe(mapType)
        map.EntityName.ShouldBe(mapName)
        map.Locations.Count.ShouldBe(mapColumns * mapRows)
        map.Locations.All(Function(x) x.EntityType = defaultLocationType).ShouldBeTrue
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

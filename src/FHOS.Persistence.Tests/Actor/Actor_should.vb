Imports System.Reflection.Metadata

Public Class Actor_should
    Private Shared Function CreateSut(
                              Optional mapName As String = "map name",
                              Optional mapType As String = "map type",
                              Optional mapColumns As Integer = 3,
                              Optional mapRows As Integer = 4,
                              Optional locationType As String = "location type",
                              Optional column As Integer = 0,
                              Optional row As Integer = 0,
                              Optional actorType As String = "actor type",
                              Optional actorName As String = "actor name",
                              Optional universe As IUniverse = Nothing,
                              Optional data As UniverseData = Nothing) As IActor
        Return CreateActor(
            mapName,
            mapType,
            mapColumns,
            mapRows,
            locationType,
            column,
            row,
            actorType,
            actorName,
            universe,
            data)
    End Function
    <Fact>
    Sub have_default_values_upon_instantiation()
        Const actorType = "actor type"
        Dim sut = CreateSut(actorType:=actorType)
        sut.EntityType.ShouldBe(actorType)
        sut.Family.ShouldNotBeNull
        sut.Properties.ShouldNotBeNull
        sut.Equipment.ShouldNotBeNull
        sut.EntityName.ShouldBe("actor name")
        sut.Location.ShouldNotBeNull
    End Sub
    <Fact>
    Sub set_entity_name()
        Const entityName = "entity name"
        Dim sut = CreateSut()
        sut.EntityName = entityName
        sut.EntityName.ShouldBe(entityName)
    End Sub
    <Fact>
    Sub by_default_have_no_relationship_to_group()
        Dim universe = CreateUniverse()
        Dim group = CreateGroup(universe:=universe)
        Dim sut = CreateSut(universe:=universe)
        sut.GroupCategory(group).ShouldBeNull
    End Sub
    <Fact>
    Sub set_relationship_to_group()
        Dim universe = CreateUniverse()
        Dim group = CreateGroup(universe:=universe)
        Dim sut = CreateSut(universe:=universe)
        Const category = "category"
        sut.GroupCategory(group) = category
        sut.GroupCategory(group).ShouldBe(category)
        sut.GroupsOfCategory(category).Count.ShouldBe(1)
    End Sub
    <Fact>
    Sub have_no_groups_in_category()
        Const category = "category"
        Dim sut = CreateSut()
        sut.GroupsOfCategory(category).ShouldBeEmpty
    End Sub
    <Fact>
    Sub have_no_yokes_to_actors()
        Const yokeType = "yoke type"
        Dim sut = CreateSut()
        sut.YokedActor(yokeType).ShouldBeNull
    End Sub
    <Fact>
    Sub allow_yoking_to_actors()
        Const yokeType = "yoke type"
        Dim sut = CreateSut()
        sut.YokedActor(yokeType) = sut
        sut.YokedActor(yokeType).Id.ShouldBe(sut.Id)
    End Sub
    <Fact>
    Sub allow_unyoking_to_actors()
        Const yokeType = "yoke type"
        Dim sut = CreateSut()
        sut.YokedActor(yokeType) = sut
        sut.YokedActor(yokeType) = Nothing
        sut.YokedActor(yokeType).ShouldBeNull
    End Sub
    <Fact>
    Sub have_no_yokes_to_stores()
        Const yokeType = "yoke type"
        Dim sut = CreateSut()
        sut.YokedStore(yokeType).ShouldBeNull
    End Sub
    <Fact>
    Sub allow_yoking_to_stores()
        Const yokeType = "yoke type"
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim store = universe.Factory.CreateStore(0)
        sut.YokedStore(yokeType) = store
        sut.YokedStore(yokeType).Id.ShouldBe(store.Id)
    End Sub
    <Fact>
    Sub allow_unyoking_to_stores()
        Const yokeType = "yoke type"
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim store = universe.Factory.CreateStore(0)
        sut.YokedStore(yokeType) = store
        sut.YokedStore(yokeType) = Nothing
    End Sub
    <Fact>
    Sub have_location()
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim location = CreateLocation(
            "other map name",
            "other map type",
            3,
            2,
            "other location type",
            1,
            1,
            universe:=universe)
        Dim oldLocation = sut.Location
        Dim actorId = oldLocation.Actor.Id
        sut.Location = location
        oldLocation.Actor.ShouldBeNull
        location.Actor.Id.ShouldBe(actorId)
    End Sub
    <Fact>
    Sub have_no_yokes_to_groups()
        Const yokeType = "yoke type"
        Dim sut = CreateSut()
        sut.YokedGroup(yokeType).ShouldBeNull
    End Sub
    <Fact>
    Sub allow_yoking_to_groups()
        Const yokeType = "yoke type"
        Const groupType = "group type"
        Const groupName = "group name"
        Dim universe = CreateUniverse()
        Dim group = universe.Factory.CreateGroup(groupType, groupName)
        Dim sut = CreateSut(universe:=universe)
        sut.YokedGroup(yokeType) = group
        sut.YokedGroup(yokeType).Id.ShouldBe(group.Id)
    End Sub
    <Fact>
    Sub allow_unyoking_to_groups()
        Const yokeType = "yoke type"
        Const groupType = "group type"
        Const groupName = "group name"
        Dim universe = CreateUniverse()
        Dim group = universe.Factory.CreateGroup(groupType, groupName)
        Dim sut = CreateSut(universe:=universe)
        sut.YokedGroup(yokeType) = group
        sut.YokedGroup(yokeType) = Nothing
        sut.YokedGroup(yokeType).ShouldBeNull
    End Sub
End Class

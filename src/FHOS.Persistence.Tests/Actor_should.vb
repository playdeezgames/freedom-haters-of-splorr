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
        sut.Tutorial.ShouldNotBeNull
        sut.Family.ShouldNotBeNull
        sut.Properties.ShouldNotBeNull
        sut.State.ShouldNotBeNull
        sut.Equipment.ShouldNotBeNull
        sut.EntityName.ShouldBe("actor name")
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
End Class

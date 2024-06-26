﻿Public Class Location_should
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

        Return CreateLocation(mapName, mapType, mapColumns, mapRows, locationType, column, row, universe, data)
    End Function

    <Fact>
    Sub have_default_values_upon_instantiation()
        Const column = 1
        Const row = 1
        Const locationType = "location type"
        Dim sut As ILocation = CreateSut(column:=column, row:=row, locationType:=locationType)
        sut.Map.ShouldNotBeNull
        sut.Position.Column.ShouldBe(column)
        sut.Position.Row.ShouldBe(row)
        sut.EntityType.ShouldBe(locationType)
        sut.Actor.ShouldBeNull
    End Sub
    <Fact>
    Sub set_entity_type()
        Const firstLocationType = "first location type"
        Const secondLocationType = "second location type"
        Dim sut = CreateSut(locationType:=firstLocationType)
        sut.EntityType.ShouldBe(firstLocationType)
        sut.EntityType = secondLocationType
        sut.EntityType.ShouldBe(secondLocationType)
    End Sub
    <Fact>
    Sub create_actor()
        Const actorType = "actor type"
        Const actorName = "actor name"
        Dim sut = CreateSut()
        Dim actor = sut.CreateActor(actorType, actorName)
        actor.ShouldNotBeNull
        actor.Location.Id.ShouldBe(sut.Id)
        sut.Actor.Id.ShouldBe(actor.Id)
        actor.EntityName.ShouldBe(actorName)
        actor.EntityType.ShouldBe(actorType)
    End Sub
End Class

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

        Return CreateLocation(mapName, mapType, mapColumns, mapRows, locationType, column, row, universe, data)
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
        actor.State.Location.Id.ShouldBe(sut.Id)
        sut.Actor.Id.ShouldBe(actor.Id)
        actor.Properties.EntityName.ShouldBe(actorName)
        actor.EntityType.ShouldBe(actorType)
    End Sub
    <Fact>
    Sub have_target_location()
        Dim sut = CreateSut()
        sut.TargetLocation = sut
        sut.HasTargetLocation.ShouldBeTrue
        sut.TargetLocation.Id.ShouldBe(sut.Id)
    End Sub
    <Fact>
    Sub have_tutorial()
        Const tutorial = "tutorial"
        Dim sut = CreateSut()
        sut.Tutorial = tutorial
        sut.Tutorial.ShouldBe(tutorial)
    End Sub
End Class

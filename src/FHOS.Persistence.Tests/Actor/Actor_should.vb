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
        sut.Equipment.ShouldNotBeNull
        sut.EntityName.ShouldBe("actor name")
        sut.Location.ShouldNotBeNull
        sut.Interior.ShouldBeNull
        sut.Costume.ShouldBeNull
        sut.Yokes.ShouldNotBeNull
    End Sub
    <Fact>
    Sub set_entity_name()
        Const entityName = "entity name"
        Dim sut = CreateSut()
        sut.EntityName = entityName
        sut.EntityName.ShouldBe(entityName)
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
    Sub have_interior()
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim map = CreateMap("other map name", "other map type", 2, 3, "other location type", universe:=universe)
        sut.Interior = map
        sut.Interior.Id.ShouldBe(map.Id)
    End Sub
    <Fact>
    Sub have_costume_type()
        Const costumeType = "costume type"
        Dim sut = CreateSut()
        sut.Costume = costumeType
        sut.Costume.ShouldBe(costumeType)
    End Sub
End Class

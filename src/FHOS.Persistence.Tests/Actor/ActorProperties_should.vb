Public Class ActorProperties_should
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
                              Optional data As UniverseData = Nothing) As IActorProperties
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
            data).Properties
    End Function
    <Fact>
    Sub have_default_values_when_instantiated()
        Const actorName = "actor name"
        Const groupType = "group type"
        Dim sut = CreateSut(actorName:=actorName)
        sut.Interior.ShouldBeNull
        sut.GetGroup(groupType).ShouldBeNull
        sut.CostumeType.ShouldBeNull
        sut.TargetActor.ShouldBeNull
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
        sut.CostumeType = costumeType
        sut.CostumeType.ShouldBe(costumeType)
    End Sub
    <Fact>
    Sub have_target_actor()
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim otherActor = CreateActor("other map name", "other map type", 3, 2, "other location type", 1, 1, "other actor type", "other actor name", universe:=universe)
        sut.TargetActor = otherActor
        sut.TargetActor.Id.ShouldBe(otherActor.Id)
        sut.TargetActor = Nothing
        sut.TargetActor.ShouldBe(Nothing)
    End Sub
End Class

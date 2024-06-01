Public Class ActorProperties_should
    Private Function CreateSut(
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
        sut.Name.ShouldBe(actorName)
        sut.Groups(groupType).ShouldBeNull
        sut.HomePlanet.ShouldBeNull
        sut.CostumeType.ShouldBeNull
        sut.TargetActor.ShouldBeNull
    End Sub
End Class

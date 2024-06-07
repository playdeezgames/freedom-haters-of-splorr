Public Class ActorState_should
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
                              Optional data As UniverseData = Nothing) As IActorState
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
            data).State
    End Function
    <Fact>
    Sub have_default_values_upon_instantiation()
        Dim sut = CreateSut()
        sut.Location.ShouldNotBeNull
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
End Class

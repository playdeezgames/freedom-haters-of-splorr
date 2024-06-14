Public Class ActorYokes_should
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
                              Optional data As UniverseData = Nothing) As IActorYokes
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
            data).Yokes
    End Function
    <Fact>
    Sub have_no_yokes_to_actors()
        Const yokeType = "yoke type"
        Dim sut = CreateSut()
        sut.YokedActor(yokeType).ShouldBeNull
    End Sub
    <Fact>
    Sub allow_yoking_to_actors()
        Const yokeType = "yoke type"
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim actor = universe.Actors.Single
        sut.YokedActor(yokeType) = actor
        sut.YokedActor(yokeType).Id.ShouldBe(actor.Id)
    End Sub
    <Fact>
    Sub allow_unyoking_to_actors()
        Const yokeType = "yoke type"
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim actor = universe.Actors.Single
        sut.YokedActor(yokeType) = actor
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

End Class

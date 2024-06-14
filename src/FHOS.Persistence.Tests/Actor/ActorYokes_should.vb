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
        sut.Actor(yokeType).ShouldBeNull
    End Sub
    <Fact>
    Sub allow_yoking_to_actors()
        Const yokeType = "yoke type"
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim actor = universe.Actors.Single
        sut.Actor(yokeType) = actor
        sut.Actor(yokeType).Id.ShouldBe(actor.Id)
    End Sub
    <Fact>
    Sub allow_unyoking_to_actors()
        Const yokeType = "yoke type"
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim actor = universe.Actors.Single
        sut.Actor(yokeType) = actor
        sut.Actor(yokeType) = Nothing
        sut.Actor(yokeType).ShouldBeNull
    End Sub
    <Fact>
    Sub have_no_yokes_to_stores()
        Const yokeType = "yoke type"
        Dim sut = CreateSut()
        sut.Store(yokeType).ShouldBeNull
    End Sub
    <Fact>
    Sub allow_yoking_to_stores()
        Const yokeType = "yoke type"
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim store = universe.Factory.CreateStore(0)
        sut.Store(yokeType) = store
        sut.Store(yokeType).Id.ShouldBe(store.Id)
    End Sub
    <Fact>
    Sub allow_unyoking_to_stores()
        Const yokeType = "yoke type"
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim store = universe.Factory.CreateStore(0)
        sut.Store(yokeType) = store
        sut.Store(yokeType) = Nothing
        sut.Store(yokeType).ShouldBeNull
    End Sub
    <Fact>
    Sub have_no_yokes_to_groups()
        Const yokeType = "yoke type"
        Dim sut = CreateSut()
        sut.Group(yokeType).ShouldBeNull
    End Sub
    <Fact>
    Sub allow_yoking_to_groups()
        Const yokeType = "yoke type"
        Const groupType = "group type"
        Const groupName = "group name"
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim group = universe.Factory.CreateGroup(groupType, groupName)
        sut.Group(yokeType) = group
        sut.Group(yokeType).Id.ShouldBe(group.Id)
    End Sub
    <Fact>
    Sub allow_unyoking_to_groups()
        Const yokeType = "yoke type"
        Const groupType = "group type"
        Const groupName = "group name"
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim group = universe.Factory.CreateGroup(groupType, groupName)
        sut.Group(yokeType) = group
        sut.Group(yokeType) = Nothing
        sut.Group(yokeType).ShouldBeNull
    End Sub
End Class

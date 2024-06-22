Public Class ActorOffers_should
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
                              Optional data As UniverseData = Nothing) As IActorOffers
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
            data).Offers
    End Function
    <Fact>
    Sub have_default_values_when_instantiated()
        Dim sut = CreateSut()
        sut.HasAny.ShouldBeFalse
        sut.All.ShouldBeEmpty
    End Sub
    <Fact>
    Sub add_offer()
        Dim sut = CreateSut()
        Const itemType = "item type"
        sut.Add(itemType)
        sut.HasAny.ShouldBeTrue
        sut.All.ShouldHaveSingleItem
        sut.All.Single.ShouldBe(itemType)
    End Sub

End Class

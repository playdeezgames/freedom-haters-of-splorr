Public Class ActorInventory_should
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
                              Optional data As UniverseData = Nothing) As IActorInventory
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
            data).Inventory
    End Function
    <Fact>
    Sub have_default_values_when_instantiated()
        Dim sut = CreateSut()
        sut.Items.Count.ShouldBe(0)
    End Sub
    <Fact>
    Sub add_and_remove_item()
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim item = universe.Factory.CreateItem("item type")
        Should.NotThrow(Sub() sut.Add(item))
        sut.Items.Count.ShouldBe(1)
        sut.Items.Single.Id.ShouldBe(item.Id)
        sut.Remove(item)
        sut.Items.Count.ShouldBe(0)
    End Sub
End Class

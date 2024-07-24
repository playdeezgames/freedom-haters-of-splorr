Public Class ActorEquipment_should
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
                              Optional data As UniverseData = Nothing) As IActorEquipment
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
            data).Equipment
    End Function
    <Fact>
    Sub have_default_values_when_instantiated()
        Dim sut = CreateSut()
        sut.All.ShouldBeEmpty
        sut.AllSlots.ShouldBeEmpty
        Const equipSlot = "equip slot"
        Should.NotThrow(Sub() sut.Equip(equipSlot, Nothing))
    End Sub
    <Fact>
    Sub equip_item()
        Dim universe = CreateUniverse()
        Dim sut = CreateSut(universe:=universe)
        Dim item = CreateItem("item type", universe:=universe)
        Const equipSlot = "equip slot"
        sut.Equip(equipSlot, item)
        sut.All.Count.ShouldBe(1)
        sut.AllSlots.Count.ShouldBe(1)
        sut.All.Single.Id.ShouldBe(item.Id)
        sut.AllSlots.Single.ShouldBe(equipSlot)
    End Sub
End Class

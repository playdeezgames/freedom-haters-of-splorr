Public Class ActorData_should
    Inherits EntityData_should(Of IActorData)
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.HasChildren.ShouldBeFalse
        sut.AllChildren.ShouldBeEmpty
        sut.AllEquipment.ShouldBeEmpty
        sut.YokedActors.ShouldBeEmpty
        sut.YokedStores.ShouldBeEmpty
        sut.YokedGroups.ShouldBeEmpty
        sut.Inventory.ShouldBeEmpty
        sut.AllItems.ShouldBeEmpty
    End Sub
    <Fact>
    Sub add_child()
        Dim sut = CreateSut()
        sut.AddChild(1)
        sut.HasChildren.ShouldBeTrue
        sut.AllChildren.ShouldHaveSingleItem
    End Sub
    <Fact>
    Sub add_equipment()
        Dim sut = CreateSut()
        sut.AddEquipment(1)
        sut.AllEquipment.ShouldHaveSingleItem
    End Sub
    <Fact>
    Sub add_item()
        Dim sut = CreateSut()
        sut.AddItem(1)
        sut.AllItems.ShouldHaveSingleItem
    End Sub
    <Fact>
    Sub remove_item()
        Dim sut = CreateSut()
        sut.AddItem(1)
        sut.RemoveItem(1)
        sut.AllItems.ShouldBeEmpty
    End Sub

    Protected Overrides Function CreateSut() As IActorData
        Return New ActorData
    End Function
End Class

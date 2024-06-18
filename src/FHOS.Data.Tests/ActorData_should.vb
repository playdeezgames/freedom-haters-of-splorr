Public Class ActorData_should
    Inherits EntityData_should(Of IActorData)
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.HasChildren.ShouldBeFalse
        sut.AllChildren.ShouldBeEmpty
        sut.Equipment.ShouldBeEmpty
        sut.YokedActors.ShouldBeEmpty
        sut.YokedStores.ShouldBeEmpty
        sut.YokedGroups.ShouldBeEmpty
        sut.Inventory.ShouldBeEmpty
    End Sub
    <Fact>
    Sub add_child()
        Dim sut = CreateSut()
        sut.AddChild(1)
        sut.HasChildren.ShouldBeTrue
        sut.AllChildren.ShouldHaveSingleItem
    End Sub

    Protected Overrides Function CreateSut() As IActorData
        Return New ActorData
    End Function
End Class

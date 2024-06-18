Public Class ActorData_should
    Inherits EntityData_should(Of IActorData)
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.HasChildren.ShouldBeFalse
        sut.Children.ShouldBeEmpty
        sut.LegacyChildren.ShouldBeEmpty
        sut.LegacyEquipment.ShouldBeEmpty
        sut.YokedActors.ShouldBeEmpty
        sut.YokedStores.ShouldBeEmpty
        sut.YokedGroups.ShouldBeEmpty
        sut.LegacyInventory.ShouldBeEmpty
    End Sub
    <Fact>
    Sub add_child()
        Dim sut = CreateSut()
        sut.AddChild(1)
        sut.HasChildren.ShouldBeTrue
        sut.Children.ShouldHaveSingleItem
    End Sub

    Protected Overrides Function CreateSut() As IActorData
        Return New ActorData
    End Function
End Class

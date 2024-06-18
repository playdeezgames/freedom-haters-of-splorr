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

    Protected Overrides Function CreateSut() As IActorData
        Return New ActorData
    End Function
End Class

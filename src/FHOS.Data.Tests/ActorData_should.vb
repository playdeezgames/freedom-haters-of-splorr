Public Class ActorData_should
    Inherits EntityData_should(Of IActorData)
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Statistics.ShouldBeEmpty
        sut.Metadatas.ShouldBeEmpty
        sut.Children.ShouldBeEmpty
        sut.Equipment.ShouldBeEmpty
        sut.YokedActors.ShouldBeEmpty
        sut.YokedStores.ShouldBeEmpty
        sut.YokedGroups.ShouldBeEmpty
        sut.Inventory.ShouldBeEmpty
    End Sub

    Protected Overrides Function CreateSut() As IActorData
        Return New ActorData
    End Function
End Class

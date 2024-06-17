Public Class ActorData_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut As IActorData = New ActorData
        sut.Flags.ShouldBeEmpty
        sut.Statistics.ShouldBeEmpty
        sut.Metadatas.ShouldBeEmpty
        sut.Children.ShouldBeEmpty
        sut.Equipment.ShouldBeEmpty
        sut.YokedActors.ShouldBeEmpty
        sut.YokedStores.ShouldBeEmpty
        sut.YokedGroups.ShouldBeEmpty
        sut.Inventory.ShouldBeEmpty
    End Sub
End Class

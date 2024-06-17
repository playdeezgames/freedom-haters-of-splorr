Public Class UniverseData_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut As IUniverseData = New UniverseData
        sut.Flags.ShouldBeEmpty
        sut.Statistics.ShouldBeEmpty
        sut.Metadatas.ShouldBeEmpty
        sut.Actors.Recycled.ShouldBeEmpty
        sut.Locations.Recycled.ShouldBeEmpty
        sut.Maps.Recycled.ShouldBeEmpty
        sut.Groups.Recycled.ShouldBeEmpty
        sut.Stores.Recycled.ShouldBeEmpty
        sut.Items.Recycled.ShouldBeEmpty
        sut.Actors.Entities.ShouldBeEmpty
        sut.Locations.Entities.ShouldBeEmpty
        sut.Maps.Entities.ShouldBeEmpty
        sut.Groups.Entities.ShouldBeEmpty
        sut.Stores.Entities.ShouldBeEmpty
        sut.Items.Entities.ShouldBeEmpty
        sut.Avatars.ShouldBeEmpty
        sut.Messages.ShouldBeEmpty
    End Sub

End Class

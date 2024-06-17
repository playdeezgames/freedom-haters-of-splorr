Public Class UniverseData_should
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut As IUniverseData = New UniverseData
        sut.Flags.ShouldBeEmpty
        sut.Statistics.ShouldBeEmpty
        sut.Metadatas.ShouldBeEmpty
        sut.Actors.ShouldBeEmpty
        sut.Locations.ShouldBeEmpty
        sut.Maps.ShouldBeEmpty
        sut.Groups.ShouldBeEmpty
        sut.Stores.ShouldBeEmpty
        sut.Items.ShouldBeEmpty
        sut.Avatars.ShouldBeEmpty
        sut.Messages.ShouldBeEmpty
    End Sub

End Class

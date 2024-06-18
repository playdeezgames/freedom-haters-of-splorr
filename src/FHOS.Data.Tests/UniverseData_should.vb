Public Class UniverseData_should
    Inherits EntityData_should(Of IUniverseData)
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Actors.ShouldBeEmpty
        sut.Locations.ShouldBeEmpty
        sut.Maps.ShouldBeEmpty
        sut.Groups.ShouldBeEmpty
        sut.Stores.ShouldBeEmpty
        sut.Items.ShouldBeEmpty
        sut.Avatars.ShouldBeEmpty
        sut.Messages.ShouldBeEmpty
    End Sub
    Protected Overrides Function CreateSut() As IUniverseData
        Return New UniverseData
    End Function
End Class

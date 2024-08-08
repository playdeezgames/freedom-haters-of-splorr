Public Class UniverseData_should
    Inherits EntityData_should(Of UniverseData)
    <Fact>
    Sub have_default_values_upon_initialization()
        Dim sut = CreateSut()
        sut.Actors.ShouldBeEmpty
        sut.Locations.ShouldBeEmpty
        sut.Maps.ShouldBeEmpty
        sut.Groups.ShouldBeEmpty
        sut.Stores.ShouldBeEmpty
        sut.Items.ShouldBeEmpty
        sut.Avatar.ShouldBeNull
        'note: NextActorId (etc) have a side effect. so don't test them.
    End Sub
    <Fact>
    Sub default_actors_to_nothing()
        Dim sut = CreateSut()
        Const actorId = 1
        sut.GetActorData(actorId).ShouldBeNull
    End Sub
    Protected Overrides Function CreateSut() As UniverseData
        Return New UniverseData
    End Function
End Class

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
        'note: NextActorId (etc) have a side effect. so don't test them.
    End Sub
    <Fact>
    Sub default_actors_to_nothing()
        Dim sut = CreateSut()
        Const actorId = 1
        sut.GetActorData(actorId).ShouldBeNull
    End Sub
    <Fact>
    Sub default_locations_to_nothing()
        Dim sut = CreateSut()
        Const locationId = 1
        sut.GetLocationData(locationId).ShouldBeNull
    End Sub
    <Fact>
    Sub default_maps_to_nothing()
        Dim sut = CreateSut()
        Const mapId = 1
        sut.GetMapData(mapId).ShouldBeNull
    End Sub
    <Fact>
    Sub default_stores_to_nothing()
        Dim sut = CreateSut()
        Const storeId = 1
        sut.GetStoreData(storeId).ShouldBeNull
    End Sub
    <Fact>
    Sub default_groups_to_nothing()
        Dim sut = CreateSut()
        Const groupId = 1
        sut.GetGroupData(groupId).ShouldBeNull
    End Sub
    <Fact>
    Sub default_items_to_nothing()
        Dim sut = CreateSut()
        Const itemId = 1
        sut.GetItemData(itemId).ShouldBeNull
    End Sub
    Protected Overrides Function CreateSut() As IUniverseData
        Return New UniverseData
    End Function
End Class

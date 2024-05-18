Friend Class StarDockDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.StarDock,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.StarDock, Hues.Brown), 1}
            },
            spawnRolls:=New Dictionary(Of String, String) From
            {
                {MapTypes.PlanetOrbit, "1d1"}
            },
            Function(x) x.LocationType = LocationTypes.Void AndAlso x.Actor Is Nothing,
            AddressOf InitializeStarDock)
    End Sub

    Private Shared Sub InitializeStarDock(actor As Persistence.IActor)
        actor.Properties.CanRefillOxygen = True
        actor.Properties.BuysScrap = True
        actor.Properties.CanRefuel = True
        Dim map = actor.State.Location.Map
        Dim place = map.GetLocation(map.Size.Columns \ 2, map.Size.Rows \ 2)?.Place
        If place IsNot Nothing Then
            actor.Properties.Faction = place.Properties.Faction
        End If
    End Sub
End Class

﻿Friend Class StarDockActorTypeDescriptor
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
            })
    End Sub

    Friend Overrides Function CanSpawn(location As Persistence.ILocation) As Boolean
        Return location.LocationType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Protected Overrides Sub Initialize(actor As Persistence.IActor)
        actor.Properties.CanRefillOxygen = True
        actor.Properties.BuysScrap = True
        actor.Properties.CanRefuel = True
        Dim map = actor.State.Location.Map
        Dim place = map.GetLocation(map.Size.Columns \ 2, map.Size.Rows \ 2)?.Place
        If place IsNot Nothing Then
            actor.Properties.Faction = place.Properties.Faction
        End If
    End Sub

    Friend Overrides Function Describe(actor As Persistence.IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {
            ($"Faction: {actor.Properties.Faction.Name}", Hues.Black)
            }
    End Function
End Class
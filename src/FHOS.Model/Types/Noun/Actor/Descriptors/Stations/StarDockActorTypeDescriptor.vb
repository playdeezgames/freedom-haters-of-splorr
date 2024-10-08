﻿Friend Class StarDockActorTypeDescriptor
    Inherits StationActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.StarDock,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.StarDock, Hues.Brown), 1}
            },
            StatisticTypes.StarDockCount,
            flags:=
            {
                FlagTypes.CanRefillOxygen,
                FlagTypes.CanRefuel
            })
    End Sub

    Friend Overrides Function GetSpawnCount(mapType As String) As Integer
        If mapType = MapTypes.PlanetOrbit Then
            Return 1
        End If
        Return MyBase.GetSpawnCount(mapType)
    End Function

    Friend Overrides Function CanSpawn(location As Persistence.ILocation) As Boolean
        Return location.EntityType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Friend Overrides Function Describe(actor As Persistence.IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {
            ($"Faction: {actor.Yokes.Group(YokeTypes.Faction).EntityName}", Hues.LightGray)
            }
    End Function

    Protected Overrides Function MakeName(planet As Persistence.IActor) As String
        Return $"{planet.EntityName} Star Dock"
    End Function

    Protected Overrides Sub Initialize(actor As Persistence.IActor)
        MyBase.Initialize(actor)
        actor.GenerateDeliveryMission
    End Sub
End Class

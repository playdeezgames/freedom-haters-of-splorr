Friend Class ShipyardActorTypeDescriptor
    Inherits StationActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.Shipyard,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.Shipyard, Hues.Orange), 1}
            },
            StatisticTypes.ShipyardCount,
            spawnRolls:=New Dictionary(Of String, String) From
            {
                {MapTypes.PlanetOrbit, "1d4/4"}
            })
    End Sub

    Protected Overrides Sub Initialize(actor As Persistence.IActor)
        MyBase.Initialize(actor)
    End Sub

    Protected Overrides Function MakeName(planet As Persistence.IActor) As String
        Return $"{planet.Yokes.YokedGroup(YokeTypes.Planet).EntityName} Shipyard"
    End Function

    Friend Overrides Function CanSpawn(location As Persistence.ILocation) As Boolean
        Return location.EntityType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Friend Overrides Function Describe(actor As Persistence.IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {
            ($"Faction: {actor.Yokes.YokedGroup(YokeTypes.Faction).EntityName}", Hues.Black)
            }
    End Function
End Class

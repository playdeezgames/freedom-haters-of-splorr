Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class StarGateActorTypeDescriptor
    Inherits StationActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.StarGate,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.StarGate, Hues.LightGreen), 1}
            },
            StatisticTypes.StarGateCount,
            flags:={FlagTypes.IsStarGate})
    End Sub

    Friend Overrides Function GetSpawnCount(mapType As String) As Integer
        If mapType = MapTypes.PlanetOrbit Then
            Return RNG.RollDice("1d4/4")
        End If
        Return MyBase.GetSpawnCount(mapType)
    End Function

    Protected Overrides Function MakeName(planet As IActor) As String
        Return $"{planet.EntityName} Star Gate"
    End Function

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return location.EntityType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {
            ($"Faction: {actor.Yokes.Group(YokeTypes.Faction).EntityName}", Hues.LightGray)
            }
    End Function
End Class

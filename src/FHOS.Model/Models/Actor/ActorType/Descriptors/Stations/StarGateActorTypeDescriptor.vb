Imports FHOS.Persistence

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
            spawnRolls:=New Dictionary(Of String, String) From
            {
                {MapTypes.PlanetOrbit, "1d4/4"}
            },
            isStarGate:=True)
    End Sub

    Protected Overrides Function MakeName(planet As IActor) As String
        Return $"{planet.EntityName} Star Gate"
    End Function

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return location.EntityType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {
            ($"Faction: {actor.Yokes.YokedGroup(YokeTypes.Faction).EntityName}", Hues.Black)
            }
    End Function
End Class

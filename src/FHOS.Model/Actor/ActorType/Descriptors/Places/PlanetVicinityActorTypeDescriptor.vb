Imports FHOS.Persistence

Friend Class PlanetVicinityActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New(planetType As String)
        MyBase.New(ActorTypes.MakePlanetVicinity(planetType),
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakePlanetVicinity(planetType), 1}
            },
            New Dictionary(Of String, String),
            isPlanetVicinity:=True,
            subtype:=planetType)
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Dim result As New List(Of (Text As String, Hue As Integer))
        Dim planetType = PlanetTypes.Descriptors(actor.Descriptor.Subtype)
        result.Add(($"Planet Type: {planetType.PlanetType}", Hues.Black))
        result.Add(($"Faction: {actor.YokedGroup(YokeTypes.PlanetVicinity).Parents.Single(Function(x) x.EntityType = GroupTypes.Faction).EntityName}", Hues.Black))
        result.Add(($"Satellites: {actor.YokedGroup(YokeTypes.PlanetVicinity).Statistics(StatisticTypes.SatelliteCount)}", Hues.Black))
        Return result
    End Function
End Class

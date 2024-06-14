Imports FHOS.Persistence

Friend Class SatelliteActorTypeDescriptor
    Inherits ActorTypeDescriptor


    Public Sub New(satelliteType As String)
        MyBase.New(
            ActorTypes.MakeSatellite(satelliteType),
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeSatellite(satelliteType), 1}
            },
            New Dictionary(Of String, String),
            isSatellite:=True,
            subtype:=satelliteType)
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Dim result As New List(Of (Text As String, Hue As Integer))
        result.Add(($"Satellite Type: {SatelliteTypes.Descriptors(actor.Descriptor.Subtype).SatelliteType}", Hues.Black))
        result.Add(($"Planet: {actor.Yokes.Group(YokeTypes.PlanetVicinity).EntityName}", Hues.Black))
        result.Add(($"Star System: {actor.Yokes.Group(YokeTypes.StarSystem).EntityName}", Hues.Black))
        Return result
    End Function
End Class

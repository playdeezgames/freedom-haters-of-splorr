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
            flags:={FlagTypes.IsSatellite},
            subtype:=satelliteType)
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Dim result As New List(Of (Text As String, Hue As Integer)) From {
            ($"Satellite Type: {SatelliteTypes.Descriptors(actor.Descriptor.Subtype).SatelliteType}", Hues.LightGray),
            ($"Tech Level: {actor.Yokes.Group(YokeTypes.Satellite).Statistics(StatisticTypes.TechLevel)}", Hues.LightGray),
            ($"Planet: {actor.Yokes.Group(YokeTypes.PlanetVicinity).EntityName}", Hues.LightGray),
            ($"Star System: {actor.Yokes.Group(YokeTypes.StarSystem).EntityName}", Hues.LightGray)
        }
        Return result
    End Function
End Class

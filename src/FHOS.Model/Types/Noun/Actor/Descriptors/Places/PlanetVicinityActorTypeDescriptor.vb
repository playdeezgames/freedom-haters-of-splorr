Imports FHOS.Persistence

Friend Class PlanetVicinityActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New(planetType As String)
        MyBase.New(ActorTypes.MakePlanetVicinity(planetType),
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakePlanetVicinity(planetType), 1}
            },
            flags:={FlagTypes.IsPlanetVicinity},
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
        result.Add(($"Planet Type: {planetType.PlanetType}", Hues.LightGray))
        Dim planetVicinityGroup = actor.Yokes.Group(YokeTypes.PlanetVicinity)
        result.Add(($"Tech Level: {planetVicinityGroup.Statistics(StatisticTypes.TechLevel)}", Hues.LightGray))
        result.Add(($"Faction: { planetVicinityGroup.SingleParent(GroupTypes.Faction).EntityName}", Hues.LightGray))
        result.Add(($"Satellites: {planetVicinityGroup.Statistics(StatisticTypes.SatelliteCount)}", Hues.LightGray))
        result.Add(($"Star Gates: {planetVicinityGroup.Statistics(StatisticTypes.StarGateCount)}", Hues.LightGray))
        result.Add(($"Ship Yards: {planetVicinityGroup.Statistics(StatisticTypes.ShipyardCount)}", Hues.LightGray))
        result.Add(($"Trading Posts: {planetVicinityGroup.Statistics(StatisticTypes.TradingPostCount)}", Hues.LightGray))
        Dim starSystemGroup = planetVicinityGroup.SingleParent(GroupTypes.StarSystem)
        result.Add(($"Star System: {starSystemGroup.EntityName}", Hues.LightGray))
        Return result
    End Function
End Class

Imports FHOS.Persistence

Friend Class StarSystemActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New(starType As String)
        MyBase.New(ActorTypes.MakeStarSystem(starType),
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeStarSystem(starType), 1}
            },
            New Dictionary(Of String, String),
            isStarSystem:=True,
            subtype:=starType)
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return True
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Dim result As New List(Of (Text As String, Hue As Integer))
        Dim descriptor = StarTypes.Descriptors(actor.Descriptor.Subtype)
        result.Add(($"Type: {descriptor.StarTypeName}", Hues.LightGray))
        Dim location = actor.Location
        result.Add(($"Galactic Position: ({location.Position.Column}, {location.Position.Row})", Hues.LightGray))
        Dim starSystemGroup = actor.Yokes.Group(YokeTypes.StarSystem)
        result.Add(($"Planets: { starSystemGroup.Statistics(StatisticTypes.PlanetCount)}", Hues.LightGray))
        result.Add(($"Satellites: {starSystemGroup.Statistics(StatisticTypes.SatelliteCount)}", Hues.LightGray))
        result.Add(($"Wormholes: {starSystemGroup.Statistics(StatisticTypes.WormholeCount)}", Hues.LightGray))
        result.Add(($"Scrap: {starSystemGroup.Statistics(StatisticTypes.Scrap)}", Hues.LightGray))
        result.Add(($"Shipyards: {starSystemGroup.Statistics(StatisticTypes.ShipyardCount)}", Hues.LightGray))
        result.Add(($"Trading Posts: {starSystemGroup.Statistics(StatisticTypes.TradingPostCount)}", Hues.LightGray))
        result.Add(($"Star Gates: {starSystemGroup.Statistics(StatisticTypes.StarGateCount)}", Hues.LightGray))
        result.Add(($"Visit Count: {starSystemGroup.Statistics(StatisticTypes.VisitCount)}", Hues.LightGray))
        result.Add(("Factions Present:", Hues.LightGray))
        For Each factionName In starSystemGroup.Children.
            Where(Function(x) x.EntityType = GroupTypes.PlanetVicinity).
            Select(Function(x) x.SingleParent(GroupTypes.Faction).EntityName).Distinct
            result.Add((factionName, Hues.LightGray))
        Next
        Return result
    End Function
End Class

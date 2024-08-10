Imports FHOS.Persistence

Friend Class TradingPostActorTypeDescriptor
    Inherits StationActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.TradingPost,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.TradingPost, Hues.Cyan), 1}
            },
            StatisticTypes.TradingPostCount,
            spawnRolls:=New Dictionary(Of String, String) From
            {
                {
                    MapTypes.PlanetOrbit,
                    "3d6/10"
                }
            },
            canTrade:=True)
    End Sub

    Protected Overrides Sub Initialize(actor As Persistence.IActor)
        MyBase.Initialize(actor)
        actor.Statistics(StatisticTypes.TechLevel) = actor.Location.Map.YokedGroup(YokeTypes.Planet).Statistics(StatisticTypes.TechLevel)
        actor.Offers.Add(ItemTypes.Scrap)
        TryAddPrice(actor, ItemTypes.OxygenTank)
        TryAddPrice(actor, ItemTypes.OxygenTank)
        TryAddPrice(actor, ItemTypes.FuelRod)
        TryAddPrice(actor, ItemTypes.AtmosphericConcentrator)
        TryAddPrice(actor, ItemTypes.FuelScoop)
        For Each markType In Marks.Descriptors.Keys
            TryAddPrice(actor, ItemTypes.MarkedType(ItemTypes.LifeSupport, markType))
            TryAddPrice(actor, ItemTypes.MarkedType(ItemTypes.FuelSupply, markType))
        Next
    End Sub

    Private Function TryAddPrice(actor As IActor, itemType As String) As Boolean
        Dim itemTechLevel = ItemTypes.Descriptors(itemType).TechLevel
        If itemTechLevel Is Nothing OrElse itemTechLevel.Value <= actor.Statistics(StatisticTypes.TechLevel).Value Then
            actor.Prices.Add(itemType)
            Return True
        End If
        Return False
    End Function

    Protected Overrides Function MakeName(planet As Persistence.IActor) As String
        Return $"{planet.EntityName} Trading Post"
    End Function

    Friend Overrides Function CanSpawn(location As Persistence.ILocation) As Boolean
        Return location.EntityType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Friend Overrides Function Describe(actor As Persistence.IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {
            ($"Faction: {actor.Yokes.Group(YokeTypes.Faction).EntityName}", Hues.LightGray)
            }
    End Function
End Class

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
        actor.Offers.Add(ItemTypes.Scrap)
        actor.Prices.Add(ItemTypes.OxygenTank)
        actor.Prices.Add(ItemTypes.FuelRod)
        actor.Prices.Add(ItemTypes.AtmosphericConcentrator)
        actor.Prices.Add(ItemTypes.FuelScoop)
        For Each markType In Marks.Descriptors.Keys
            actor.Prices.Add(ItemTypes.MarkedType(ItemTypes.LifeSupport, markType))
            actor.Prices.Add(ItemTypes.MarkedType(ItemTypes.FuelSupply, markType))
        Next
    End Sub

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

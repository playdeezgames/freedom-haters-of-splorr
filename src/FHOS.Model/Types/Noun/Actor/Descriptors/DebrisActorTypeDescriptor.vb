Imports SPLORR.Game

Friend Class DebrisActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.Debris,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.Debris, Hues.DarkGray), 1}
            },
            spawnRolls:=New Dictionary(Of String, String) From
            {
                {MapTypes.StarSystem, "12d6/6"}
            },
            flags:={FlagTypes.CanSalvage})
    End Sub

    Protected Overrides Sub Initialize(actor As Persistence.IActor)
        Dim scrapCount = RNG.RollDice("4d6")
        For Each dummy In Enumerable.Range(0, scrapCount)
            actor.Inventory.Add(actor.Universe.Factory.CreateItem(ItemTypes.Scrap))
        Next
        Dim starSystemGroup = actor.Location.Map.YokedGroup(YokeTypes.StarSystem)
        starSystemGroup.Statistics(StatisticTypes.Scrap) += 1
    End Sub

    Friend Overrides Function CanSpawn(location As Persistence.ILocation) As Boolean
        Return location.EntityType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Friend Overrides Function Describe(actor As Persistence.IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return Array.Empty(Of (Text As String, Hue As Integer))
    End Function
End Class

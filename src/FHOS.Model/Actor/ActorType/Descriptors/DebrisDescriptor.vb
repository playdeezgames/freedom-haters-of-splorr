Imports SPLORR.Game

Friend Class DebrisDescriptor
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
            canSpawn:=Function(x) x.LocationType = Void AndAlso x.Actor Is Nothing,
            initializer:=AddressOf InitializeDebris)
    End Sub

    Private Shared Sub InitializeDebris(actor As Persistence.IActor)
        actor.Properties.CanSalvage = True
        actor.State.Scrap = RNG.RollDice("4d6")
    End Sub

    Friend Overrides Function CanSpawn(location As Persistence.ILocation) As Boolean
        Return location.LocationType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function
End Class

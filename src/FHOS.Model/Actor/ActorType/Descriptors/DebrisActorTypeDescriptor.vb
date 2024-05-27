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
            canSalvage:=True)
    End Sub

    Protected Overrides Sub Initialize(actor As Persistence.IActor)
        actor.State.Scrap = RNG.RollDice("4d6")
    End Sub

    Friend Overrides Function CanSpawn(location As Persistence.ILocation) As Boolean
        Return location.LocationType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function

    Friend Overrides Function Describe(actor As Persistence.IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return Array.Empty(Of (Text As String, Hue As Integer))
    End Function
End Class

Imports System.Data
Imports FHOS.Persistence

Friend Class WormholeActorTypeDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.Wormhole,
            New Dictionary(Of String, Integer) From
            {
                {MakeCostume(CostumeTypes.Wormhole, Hues.Purple), 1}
            },
            New Dictionary(Of String, String))
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
        Dim location = actor.State.Location
        location.LocationType = LocationTypes.Wormhole
        For Each neighbor In location.GetNeighbors().Where(Function(x) x.LocationType = LocationTypes.Void)
            neighbor.LocationType = LocationTypes.ActorAdjacent
        Next
        actor.Properties.IsWormhole = True
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return location.LocationType = LocationTypes.Void AndAlso
            location.Actor Is Nothing AndAlso
            location.GetNeighbors().Any(Function(n) n.LocationType = LocationTypes.Void AndAlso n.Actor Is Nothing)
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {
            ("It's a wormhole!", Hues.Black),
            ("No actual worms were involved, we don't think.", Hues.Black),
            ("Instead, it is a space-time anomaly that allows ships to instantly travel between one end and the other.", Hues.Black)
            }
    End Function
End Class

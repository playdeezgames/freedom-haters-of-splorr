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
            New Dictionary(Of String, String),
            isWormhole:=True)
    End Sub

    Protected Overrides Sub Initialize(actor As IActor)
        Dim location = actor.State.Location
        location.EntityType = LocationTypes.Wormhole
        For Each neighbor In location.GetEmptyNeighborsOfType(LocationTypes.Void)
            neighbor.EntityType = LocationTypes.ActorAdjacent
        Next
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return location.EntityType = LocationTypes.Void AndAlso
            location.Actor Is Nothing AndAlso
            location.GetEmptyNeighborsOfType(LocationTypes.Void).Any
    End Function

    Friend Overrides Function Describe(actor As IActor) As IEnumerable(Of (Text As String, Hue As Integer))
        Return {
            ("It's a wormhole!", Hues.Black),
            ("No actual worms were involved, we don't think. Tho, there are a few fringe folks who hypothesize that these were made by trans-dimensional space worms.", Hues.Black),
            ("Instead, it is a space-time anomaly that allows ships to instantly travel between one end and the other.", Hues.Black)
            }
    End Function
End Class

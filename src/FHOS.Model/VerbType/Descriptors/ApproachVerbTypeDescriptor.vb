Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class ApproachVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Private ReadOnly placeType As String

    Friend Sub New(verbType As String, text As String, placeType As String)
        MyBase.New(
            verbType,
            text)
        Me.placeType = placeType
    End Sub

    Protected Overrides Sub OnPerform(actor As IActor)
        DoTurn(actor)
        With actor.State.Location.Place
            SetLocation(actor, RNG.FromEnumerable(.Properties.Map.Locations.Where(Function(x) x.Flags(.Properties.Identifier) AndAlso x.Actor Is Nothing)))
        End With
    End Sub

    Friend Overrides Function IsAvailable(actor As Persistence.IActor) As Boolean
        Return actor.State.Location.Place?.PlaceType = PlaceType
    End Function
End Class

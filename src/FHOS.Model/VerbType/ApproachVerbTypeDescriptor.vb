Imports SPLORR.Game

Friend Class ApproachVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Friend Sub New(verbType As String, text As String, placeType As String)
        MyBase.New(
            verbType,
            text,
            Function(Actor)
                Return Actor.State.Location.Place?.PlaceType = placeType
            End Function)
    End Sub

    Friend Overrides Sub Perform(actor As Persistence.IActor)
        DoTurn(actor)
        With actor.State.Location.Place
            SetLocation(actor, RNG.FromEnumerable(.Properties.Map.Locations.Where(Function(x) x.Flags(.Properties.Identifier) AndAlso x.Actor Is Nothing)))
        End With
    End Sub
End Class

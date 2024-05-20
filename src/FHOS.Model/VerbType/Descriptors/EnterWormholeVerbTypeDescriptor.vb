Imports FHOS.Persistence

Friend Class EnterWormholeVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Friend Sub New()
        MyBase.New(VerbTypes.EnterWormhole, "Enter Wormhole")
    End Sub

    Protected Overrides Sub OnPerform(actor As IActor)
        DoTurn(actor)
        With actor.State.Location.Place
            SetLocation(actor, .Properties.WormholeDestination)
        End With
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.State.Location.Place?.PlaceType = PlaceTypes.Wormhole
    End Function
End Class

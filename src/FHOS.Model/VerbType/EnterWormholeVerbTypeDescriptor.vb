Imports FHOS.Persistence

Friend Class EnterWormholeVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Friend Sub New()
        MyBase.New(VerbTypes.EnterWormhole, "Enter Wormhole", Function(Actor)
                                                                  Return Actor.State.Location.Place?.PlaceType = PlaceTypes.Wormhole
                                                              End Function)
    End Sub

    Friend Overrides Sub Perform(actor As IActor)
        DoTurn(actor)
        With actor.State.Location.Place
            SetLocation(actor, .Properties.WormholeDestination)
        End With
    End Sub
End Class

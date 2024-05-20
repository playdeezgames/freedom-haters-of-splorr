Friend Class EnterWormholeInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        Dim destination = actor.State.Interactor.Properties.OtherWormhole.Properties.WormholeExit
        If destination.Actor IsNot Nothing Then
            actor.Universe.Messages.Add("Destination Blocked!", ("Other end of the wormhole is blocked!", Hues.Black))
            actor.State.Interactor = Nothing
            Return
        End If
        SetLocation(actor, destination)
        actor.State.Interactor = Nothing
    End Sub
End Class

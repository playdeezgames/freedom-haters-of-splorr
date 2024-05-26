Imports SPLORR.Game

Friend Class TransportInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        actor.GoToOtherActor(
            actor.State.Interactor.Properties.TargetActor,
            Sub(success)
                If Not success Then
                    actor.Universe.Messages.Add("Destination Blocked!", ("The other end is blocked!", Hues.Red))
                End If
                actor.State.Interactor = Nothing
            End Sub)
    End Sub
End Class

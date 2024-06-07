Imports SPLORR.Game

Friend Class TransportToActorInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        actor.GoToOtherActor(
            actor.YokedActor(YokeTypes.Interactor).Properties.TargetActor,
            Sub(success)
                If Not success Then
                    actor.Universe.Messages.Add("Destination Blocked!", ("The other end is blocked!", Hues.Red))
                End If
                actor.YokedActor(YokeTypes.Interactor) = Nothing
            End Sub)
    End Sub
End Class

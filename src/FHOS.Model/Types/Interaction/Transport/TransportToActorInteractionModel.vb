Imports SPLORR.Game

Friend Class TransportToActorInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        actor.GoToOtherActor(
            actor.Interactor.Yokes.YokedActor(YokeTypes.Target),
            Sub(success, otherActor)
                If Not success Then
                    actor.Universe.Messages.Add("Destination Blocked!", ("The other end is blocked!", Hues.Red))
                Else
                    If otherActor.Descriptor.IsStarSystem Then
                        actor.SetStarSystem(Nothing)
                    ElseIf otherActor.Descriptor.IsWormhole Then
                        actor.SetStarSystem(otherActor.Yokes.YokedGroup(YokeTypes.StarSystem))
                    End If
                End If
                actor.ClearInteractor()
            End Sub)
    End Sub
End Class

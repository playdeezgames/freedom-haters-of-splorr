Friend Class EnterStarGateInteractionModel
    Implements IInteractionModel

    Private ReadOnly actor As Persistence.IActor

    Public Sub New(actor As Persistence.IActor)
        Me.actor = actor
    End Sub

    Public Sub Perform() Implements IInteractionModel.Perform
        actor.YokedActor(YokeTypes.StarGate) = actor.YokedActor(YokeTypes.Interactor)
        actor.YokedActor(YokeTypes.Interactor) = Nothing
    End Sub
End Class

Friend Class GoodByeInteractionModel
    Implements IInteractionModel

    Private actor As Persistence.IActor

    Public Sub New(actor As Persistence.IActor)
        Me.actor = actor
    End Sub

    Public Sub Perform() Implements IInteractionModel.Perform
        actor.State.Interactor = Nothing
    End Sub
End Class

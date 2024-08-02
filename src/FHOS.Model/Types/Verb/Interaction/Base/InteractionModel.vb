Imports FHOS.Persistence

Friend Class InteractionModel
    Implements IInteractionModel

    Protected ReadOnly actor As IActor
    Private ReadOnly onPerform As Action(Of IActor)

    Sub New(actor As IActor, onPerform As Action(Of IActor))
        Me.actor = actor
        Me.onPerform = onPerform
    End Sub

    Public Sub Perform() Implements IInteractionModel.Perform
        onPerform(actor)
    End Sub
End Class

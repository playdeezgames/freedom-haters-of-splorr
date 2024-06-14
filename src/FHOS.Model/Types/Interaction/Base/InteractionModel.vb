Imports FHOS.Persistence

Friend MustInherit Class InteractionModel
    Implements IInteractionModel

    Protected ReadOnly actor As IActor

    Sub New(actor As IActor)
        Me.actor = actor
    End Sub

    Public MustOverride Sub Perform() Implements IInteractionModel.Perform
End Class

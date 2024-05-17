Imports FHOS.Persistence

Friend MustInherit Class InteractionDescriptor
    ReadOnly Property InteractionType As String
    ReadOnly Property Text As String
    Sub New(interactionType As String, text As String)
        Me.InteractionType = interactionType
        Me.Text = text
    End Sub

    Friend MustOverride Function IsAvailable(actor As IActor) As Boolean

    Friend MustOverride Function ToInteraction(actor As IActor) As IInteractionModel
End Class

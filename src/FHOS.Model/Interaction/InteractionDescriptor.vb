Imports FHOS.Persistence

Friend MustInherit Class InteractionDescriptor
    Friend ReadOnly Property InteractionType As String
    Sub New(interactionType As String)
        Me.InteractionType = interactionType
    End Sub
    Friend MustOverride Function GetText(actor As IActor) As String
    Friend MustOverride Function IsAvailable(actor As IActor) As Boolean
    Friend MustOverride Function ToInteraction(actor As IActor) As IInteractionModel
End Class

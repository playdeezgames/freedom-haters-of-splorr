Imports FHOS.Persistence

Friend Class CancelInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.Cancel)
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return True
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New InteractionModel(actor, Sub(a) a.ClearInteractor)
    End Function

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Cancel"
    End Function
End Class

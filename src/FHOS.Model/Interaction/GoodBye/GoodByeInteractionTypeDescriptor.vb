Imports FHOS.Persistence

Friend Class GoodByeInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.GoodBye)
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return True
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New GoodByeInteractionModel(actor)
    End Function

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Good-Bye"
    End Function
End Class

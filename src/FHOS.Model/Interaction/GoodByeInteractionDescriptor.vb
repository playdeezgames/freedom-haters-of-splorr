Imports FHOS.Persistence

Friend Class GoodByeInteractionDescriptor
    Inherits InteractionDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.GoodBye, "Good-Bye")
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return True
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return Nothing
    End Function
End Class

Imports FHOS.Persistence

Friend Class RefuelInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.Refuel)
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.Interactor.Descriptor.CanRefuel
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New RefuelInteractionModel(actor)
    End Function

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Refuel"
    End Function
End Class

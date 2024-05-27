Imports FHOS.Persistence

Friend Class EnterStarGateInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.EnterStarGate)
    End Sub

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Enter Star Gate"
    End Function

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.State.Interactor.Descriptor.IsStarGate
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New EnterStarGateInteractionModel(actor)
    End Function
End Class

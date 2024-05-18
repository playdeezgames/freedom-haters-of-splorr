Imports FHOS.Persistence

Friend Class RefillOxygenInteractionDescriptor
    Inherits InteractionDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.RefillOxygen, "Refill Oxygen")
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.State.Interactor.CanRefillOxygen
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New RefillOxygenInteractionModel(actor)
    End Function
End Class

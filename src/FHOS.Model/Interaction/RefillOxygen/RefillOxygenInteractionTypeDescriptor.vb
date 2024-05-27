Imports FHOS.Persistence

Friend Class RefillOxygenInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.RefillOxygen)
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.NeedsOxygen AndAlso ActorTypes.Descriptors(actor.State.Interactor.ActorType).CanRefillOxygen
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New RefillOxygenInteractionModel(actor)
    End Function

    Friend Overrides Function GetText(actor As IActor) As String
        Return $"Refill Oxygen (Currently {actor.State.LifeSupport.Percent}%)"
    End Function
End Class

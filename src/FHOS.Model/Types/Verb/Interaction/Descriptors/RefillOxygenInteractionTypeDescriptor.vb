Imports FHOS.Persistence

Friend Class RefillOxygenInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.RefillOxygen)
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.NeedsOxygen AndAlso actor.Interactor.Flags(FlagTypes.CanRefillOxygen)
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New InteractionModel(actor, Sub(a)
                                               a.Dialog = New OxygenRefilledDialog(a, a.Dialog)
                                           End Sub)
    End Function

    Friend Overrides Function GetText(actor As IActor) As String
        'TODO: give me the price!
        Return $"Refill Oxygen (Currently {actor.LifeSupport.Percent}%)"
    End Function
End Class

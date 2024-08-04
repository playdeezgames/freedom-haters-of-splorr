Imports FHOS.Persistence

Friend Class RefillOxygenInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.RefillOxygen)
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.NeedsOxygen AndAlso actor.Interactor.Descriptor.CanRefillOxygen
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New InteractionModel(actor, Sub(a)
                                               Dim oxygenRequired = a.LifeSupport.TopOffAmount.Value
                                               Const oxygenPerJools = 10
                                               Dim oxygenCost = (oxygenRequired + oxygenPerJools - 1) \ oxygenPerJools
                                               a.Yokes.Store(YokeTypes.Wallet).CurrentValue -= oxygenCost
                                               a.LifeSupport.CurrentValue += oxygenRequired
                                               a.Universe.Messages.Add("Oxygen Refilled!", ($"You buy {oxygenRequired} oxygen!", Black), ($"Cost: {oxygenCost} Jools!", Black))
                                           End Sub)
    End Function

    Friend Overrides Function GetText(actor As IActor) As String
        'TODO: give me the price!
        Return $"Refill Oxygen (Currently {actor.LifeSupport.Percent}%)"
    End Function
End Class

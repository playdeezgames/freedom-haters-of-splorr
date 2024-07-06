Friend Class RefillOxygenInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor, AddressOf DoRefillOxygen)
    End Sub

    Private Shared Sub DoRefillOxygen(actor As Persistence.IActor)
        Dim oxygenRequired = actor.Yokes.Store(YokeTypes.LifeSupport).TopOffAmount.Value
        Const oxygenPerJools = 10
        Dim oxygenCost = (oxygenRequired + oxygenPerJools - 1) \ oxygenPerJools
        actor.Yokes.Store(YokeTypes.Wallet).CurrentValue -= oxygenCost
        actor.Yokes.Store(YokeTypes.LifeSupport).CurrentValue += oxygenRequired
        actor.Universe.Messages.Add("Oxygen Refilled!", ($"You buy {oxygenRequired} oxygen!", Black), ($"Cost: {oxygenCost} Jools!", Black))
    End Sub
End Class

Friend Class RefillOxygenInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        actor.State.LifeSupport.CurrentValue = actor.State.LifeSupport.MaximumValue.Value
        actor.State.Wallet.CurrentValue -= 1
        actor.Universe.Messages.Add("Oxygen Refilled!", ("You refill yer oxygen!", Black), ("Cost: 1 Jools!", Black))
    End Sub
End Class

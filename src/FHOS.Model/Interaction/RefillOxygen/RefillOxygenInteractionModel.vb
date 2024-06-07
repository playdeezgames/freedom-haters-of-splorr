Friend Class RefillOxygenInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        actor.YokedStore(YokeTypes.LifeSupport).CurrentValue = actor.YokedStore(YokeTypes.LifeSupport).MaximumValue.Value
        actor.YokedStore(YokeTypes.Wallet).CurrentValue -= 1
        actor.Universe.Messages.Add("Oxygen Refilled!", ("You refill yer oxygen!", Black), ("Cost: 1 Jools!", Black))
    End Sub
End Class

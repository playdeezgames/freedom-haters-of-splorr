﻿Friend Class RefillOxygenInteractionModel
    Implements IInteractionModel

    Private ReadOnly actor As Persistence.IActor

    Public Sub New(actor As Persistence.IActor)
        Me.actor = actor
    End Sub

    Public Sub Perform() Implements IInteractionModel.Perform
        actor.State.LifeSupport.CurrentValue = actor.State.LifeSupport.MaximumValue.Value
        actor.State.Wallet.CurrentValue -= 1
        actor.Universe.Messages.Add("Oxygen Refilled!", ("You refill yer oxygen!", Black), ("Cost: 1 Jools!", Black))
    End Sub
End Class
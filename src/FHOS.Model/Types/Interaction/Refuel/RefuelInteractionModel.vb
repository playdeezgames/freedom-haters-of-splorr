﻿Friend Class RefuelInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        Dim fuelRequired = actor.Yokes.Store(YokeTypes.FuelTank).TopOffAmount.Value
        Const fuelPerJools = 10
        Dim fuelCost = (fuelRequired + fuelPerJools - 1) \ fuelPerJools
        actor.Yokes.Store(YokeTypes.Wallet).CurrentValue -= fuelCost
        actor.Yokes.Store(YokeTypes.FuelTank).CurrentValue += fuelRequired
        actor.Universe.Messages.Add("Refueled!", ($"You bought {fuelRequired} fuel.", Black), ($"You paid {fuelCost} Jools.", Black))
    End Sub
End Class

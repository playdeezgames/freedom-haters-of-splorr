Friend Class RefuelInteractionModel
    Inherits InteractionModel

    Public Sub New(actor As Persistence.IActor)
        MyBase.New(actor)
    End Sub

    Public Overrides Sub Perform()
        Dim fuelRequired = actor.State.FuelTank.MaximumValue.Value - actor.State.FuelTank.CurrentValue
        actor.State.Wallet.CurrentValue -= fuelRequired
        actor.State.FuelTank.CurrentValue += fuelRequired
        actor.Universe.Messages.Add("Refueled!", ($"You bought {fuelRequired} fuel.", Black), ($"You paid {fuelRequired} Jools.", Black))
    End Sub
End Class

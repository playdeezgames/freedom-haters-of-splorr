Friend Class RefuelInteractionModel
    Implements IInteractionModel

    Private ReadOnly actor As Persistence.IActor

    Public Sub New(actor As Persistence.IActor)
        Me.actor = actor
    End Sub

    Public Sub Perform() Implements IInteractionModel.Perform
        Dim fuelRequired = actor.State.FuelTank.MaximumValue.Value - actor.State.FuelTank.CurrentValue
        actor.State.Wallet.CurrentValue -= fuelRequired
        actor.State.FuelTank.CurrentValue += fuelRequired
        actor.Universe.Messages.Add("Refueled!", ($"You bought {fuelRequired} fuel.", Black), ($"You paid {fuelRequired} Jools.", Black))
    End Sub
End Class

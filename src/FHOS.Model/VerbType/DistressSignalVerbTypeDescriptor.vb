Imports FHOS.Persistence

Friend Class DistressSignalVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Friend Sub New()
        MyBase.New(DistressSignal, "Signal Distress")
    End Sub

    Friend Overrides Sub Perform(actor As IActor)
        Dim fuelAdded = actor.State.FuelTank.MaximumValue.Value - actor.State.FuelTank.CurrentValue
        Dim fuelPrice = 1 'TODO: don't just pick a magic number!
        Dim price = fuelPrice * fuelAdded
        actor.State.FuelTank.CurrentValue = actor.State.FuelTank.MaximumValue.Value
        actor.State.Wallet.CurrentValue -= fuelAdded * fuelPrice
        actor.Universe.Messages.Add(
                        "Emergency Refuel",
                        ($"Added {fuelAdded} fuel!", Hues.Black),
                        ($"Price {price} jools!", Hues.Black))
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.State.FuelTank IsNot Nothing AndAlso AvatarModel.FromActor(actor).Vessel.FuelPercent.Value = 0
    End Function
End Class

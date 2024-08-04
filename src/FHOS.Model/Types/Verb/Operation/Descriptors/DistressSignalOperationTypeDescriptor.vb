Imports FHOS.Persistence

Friend Class DistressSignalOperationTypeDescriptor
    Inherits OperationTypeDescriptor

    Friend Sub New()
        MyBase.New(DistressSignal, "Signal Distress")
    End Sub

    Protected Overrides Sub OnPerform(actor As IActor)
        Dim fuelAdded = actor.FuelTank.MaximumValue.Value - actor.FuelTank.CurrentValue
        Dim fuelPrice = 1 'TODO: don't just pick a magic number!
        Dim price = fuelPrice * fuelAdded
        actor.FuelTank.CurrentValue = actor.FuelTank.MaximumValue.Value
        actor.Yokes.Store(YokeTypes.Wallet).CurrentValue -= fuelAdded * fuelPrice
        actor.Universe.Messages.Add(
                        "Emergency Refuel",
                        ($"Added {fuelAdded} fuel!", Hues.Black),
                        ($"Price {price} jools!", Hues.Black))
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.FuelTank IsNot Nothing AndAlso AvatarModel.FromActor(actor).Vessel.FuelPercent.Value = 0
    End Function
End Class

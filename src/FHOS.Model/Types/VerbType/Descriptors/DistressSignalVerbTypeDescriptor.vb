Imports FHOS.Persistence

Friend Class DistressSignalVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Friend Sub New()
        MyBase.New(DistressSignal, "Signal Distress")
    End Sub

    Protected Overrides Sub OnPerform(actor As IActor)
        Dim fuelAdded = actor.Yokes.Store(YokeTypes.FuelTank).MaximumValue.Value - actor.Yokes.Store(YokeTypes.FuelTank).CurrentValue
        Dim fuelPrice = 1 'TODO: don't just pick a magic number!
        Dim price = fuelPrice * fuelAdded
        actor.Yokes.Store(YokeTypes.FuelTank).CurrentValue = actor.Yokes.Store(YokeTypes.FuelTank).MaximumValue.Value
        actor.Yokes.Store(YokeTypes.Wallet).CurrentValue -= fuelAdded * fuelPrice
        actor.Universe.Messages.Add(
                        "Emergency Refuel",
                        ($"Added {fuelAdded} fuel!", Hues.Black),
                        ($"Price {price} jools!", Hues.Black))
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.Yokes.Store(YokeTypes.FuelTank) IsNot Nothing AndAlso AvatarModel.FromActor(actor).Vessel.FuelPercent.Value = 0
    End Function
End Class

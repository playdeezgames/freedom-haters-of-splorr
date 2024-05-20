Imports FHOS.Persistence

Friend Class DistressSignalVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Friend Sub New()
        MyBase.New(DistressSignal, "Signal Distress", Function(Actor) Actor.State.FuelTank IsNot Nothing AndAlso AvatarModel.FromActor(Actor).Vessel.FuelPercent.Value = 0,
                Sub(actor)
                    Dim fuelAdded = actor.State.FuelTank.MaximumValue.Value - actor.State.FuelTank.CurrentValue
                    Dim fuelPrice = 1 'TODO: don't just pick a magic number!
                    Dim price = fuelPrice * fuelAdded
                    actor.State.FuelTank.CurrentValue = actor.State.FuelTank.MaximumValue.Value
                    actor.State.Wallet.CurrentValue -= fuelAdded * fuelPrice
                    actor.Universe.Messages.Add(
                        "Emergency Refuel",
                        ($"Added {fuelAdded} fuel!", Hues.Black),
                        ($"Price {price} jools!", Hues.Black))
                End Sub)
    End Sub
End Class

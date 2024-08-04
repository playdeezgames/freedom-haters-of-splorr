Imports FHOS.Persistence

Friend Class RefuelInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.Refuel)
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.Interactor.Descriptor.CanRefuel
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New InteractionModel(actor, Sub(a)
                                               Dim fuelRequired = a.FuelTank.TopOffAmount.Value
                                               Const fuelPerJools = 3
                                               Dim fuelCost = (fuelRequired + fuelPerJools - 1) \ fuelPerJools
                                               a.Yokes.Store(YokeTypes.Wallet).CurrentValue -= fuelCost
                                               a.FuelTank.CurrentValue += fuelRequired
                                               a.Universe.Messages.Add("Refueled!", ($"You bought {fuelRequired} fuel.", Black), ($"You paid {fuelCost} Jools.", Black))
                                           End Sub)
    End Function

    Friend Overrides Function GetText(actor As IActor) As String
        'TODO: give me the price!
        Return "Refuel"
    End Function
End Class

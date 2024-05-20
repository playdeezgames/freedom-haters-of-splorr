Imports FHOS.Persistence

Friend Class RefuelVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Friend Sub New()
        MyBase.New(
            VerbTypes.Refuel,
            "Refuel")
    End Sub

    Protected Overrides Sub OnPerform(actor As IActor)
        actor.State.FuelTank.CurrentValue = actor.State.FuelTank.MaximumValue.Value
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.State.Location.Place?.PlaceType = PlaceTypes.Star
    End Function
End Class

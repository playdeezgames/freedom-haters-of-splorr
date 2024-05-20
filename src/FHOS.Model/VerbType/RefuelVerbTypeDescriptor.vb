Imports System.Numerics
Imports FHOS.Persistence

Friend Class RefuelVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Friend Sub New()
        MyBase.New(
            VerbTypes.Refuel,
            "Refuel",
            Function(Actor)
                Return Actor.State.Location.Place?.PlaceType = PlaceTypes.Star
            End Function)
    End Sub

    Friend Overrides Sub Perform(actor As IActor)
        actor.State.FuelTank.CurrentValue = actor.State.FuelTank.MaximumValue.Value
    End Sub
End Class

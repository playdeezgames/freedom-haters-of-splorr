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
            End Function,
            Sub(actor)
                actor.State.FuelTank.CurrentValue = actor.State.FuelTank.MaximumValue.Value
            End Sub)
    End Sub
End Class

Imports FHOS.Persistence

Friend Class RefillOxygenVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Friend Sub New()
        MyBase.New(VerbTypes.RefillOxygen, "Refill Oxygen")
    End Sub

    Protected Overrides Sub OnPerform(actor As IActor)
        actor.State.LifeSupport.CurrentValue = actor.State.LifeSupport.MaximumValue.Value
    End Sub

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Dim placeType = actor.State.Location.Place?.PlaceType
        If placeType <> PlaceTypes.Planet Then
            Return False
        End If
        Return PlanetTypes.Descriptors(actor.State.Location.Place.Subtype).CanRefillOxygen
    End Function
End Class

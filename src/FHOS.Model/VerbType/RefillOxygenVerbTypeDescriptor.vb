Imports FHOS.Persistence

Friend Class RefillOxygenVerbTypeDescriptor
    Inherits VerbTypeDescriptor

    Friend Sub New()
        MyBase.New(VerbTypes.RefillOxygen, "Refill Oxygen", Function(Actor)
                                                                Dim placeType = Actor.State.Location.Place?.PlaceType
                                                                If placeType <> PlaceTypes.Planet Then
                                                                    Return False
                                                                End If
                                                                Return PlanetTypes.Descriptors(Actor.State.Location.Place.Subtype).CanRefillOxygen
                                                            End Function,
                Sub(actor)
                    actor.State.LifeSupport.CurrentValue = actor.State.LifeSupport.MaximumValue.Value

                End Sub)
    End Sub
End Class

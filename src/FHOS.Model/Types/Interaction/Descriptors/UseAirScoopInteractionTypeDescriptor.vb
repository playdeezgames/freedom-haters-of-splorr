Imports FHOS.Persistence

Friend Class UseAirScoopInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.UseAirScoop)
    End Sub

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Use Atmospheric Concentrator..."
    End Function

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.Interactor.Descriptor.IsPlanetSection AndAlso actor.Equipment.All.Any(Function(x) x.Flags(FlagTypes.CanRefillOxygen))
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New InteractionModel(actor, Sub(a)
                                               Dim store = a.Yokes.Store(YokeTypes.LifeSupport)
                                               store.CurrentValue = store.MaximumValue.Value
                                               a.ClearInteractor()
                                           End Sub)
    End Function
End Class

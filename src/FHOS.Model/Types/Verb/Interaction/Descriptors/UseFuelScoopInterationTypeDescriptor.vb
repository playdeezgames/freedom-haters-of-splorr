Imports FHOS.Persistence

Friend Class UseFuelScoopInterationTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.UseFuelScoop)
    End Sub

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Use fuel scoop"
    End Function

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.Interactor.Descriptor.IsStar AndAlso actor.Equipment.All.Any(Function(x) x.Flags(FlagTypes.CanRefuel))
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New InteractionModel(actor, Sub(a)
                                               Dim store = a.Yokes.Store(YokeTypes.FuelTank)
                                               store.CurrentValue = store.MaximumValue.Value
                                               a.ClearInteractor()
                                           End Sub)
    End Function
End Class

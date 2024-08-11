Imports FHOS.Persistence

Friend Class TradeInteractionTypeDescriptor
    Inherits InteractionTypeDescriptor

    Public Sub New()
        MyBase.New(InteractionTypes.Trade)
    End Sub

    Friend Overrides Function GetText(actor As IActor) As String
        Return "Trade"
    End Function

    Friend Overrides Function IsAvailable(actor As IActor) As Boolean
        Return actor.Interactor.Descriptor.Flag(FlagTypes.CanTrade)
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        Return New InteractionModel(actor, Sub(a)
                                               a.Yokes.Actor(YokeTypes.Trader) = a.Interactor()
                                               a.ClearInteractor()
                                           End Sub)
    End Function
End Class

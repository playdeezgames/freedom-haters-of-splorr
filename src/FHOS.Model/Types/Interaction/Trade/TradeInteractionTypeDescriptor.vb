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
        Return actor.Interactor.Descriptor.CanTrade
    End Function

    Friend Overrides Function ToInteraction(actor As IActor) As IInteractionModel
        actor.Yokes.Actor(YokeTypes.Trader) = actor.Interactor
        Return New TradeInteractionModel(actor)
    End Function
End Class

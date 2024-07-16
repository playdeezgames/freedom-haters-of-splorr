Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class SellConfirmState
    Inherits BaseState
    Implements IState

    Private ReadOnly offer As IAvatarTraderOfferModel
    Private ReadOnly quantity As Integer

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState,
                  offer As IAvatarTraderOfferModel,
                  quantity As Integer)
        MyBase.New(model, ui, endState)
        Me.offer = offer
        Me.quantity = quantity
    End Sub

    Public Overrides Function Run() As IState
        If ui.Confirm((Mood.Prompt, $"Sell {quantity} {offer.Name} for {offer.JoolsOffered(quantity)} Jools?")) Then
            offer.Sell(quantity)
            Return New OffersState(model, ui, endState)
        End If
        Return New SellQuantityState(model, ui, endState, offer)
    End Function
End Class

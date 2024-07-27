Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class BuyConfirmState
    Inherits BaseState
    Implements IState

    Private ReadOnly price As IAvatarTraderPriceModel
    Private ReadOnly quantity As Integer

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState,
                  price As IAvatarTraderPriceModel,
                  quantity As Integer)
        MyBase.New(model, ui, endState)
        Me.price = price
        Me.quantity = quantity
    End Sub

    Public Overrides Function Run() As IState
        If ui.Confirm((Mood.Prompt, $"Buy {quantity} {price.Name} for {price.UnitPrice * quantity} Jools?")) Then
            price.Buy(quantity)
            Return New PricesState(model, ui, endState, String.Empty)
        End If
        Return New BuyQuantityState(model, ui, endState, price)
    End Function
End Class

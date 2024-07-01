Imports FHOS.Model
Imports SPLORR.UI

Friend Class PricesState
    Inherits BasePickerState(Of IUniverseModel, IAvatarTraderPriceModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Prices",
            context.ChooseOrCancel,
            GameState.Trader)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IAvatarTraderPriceModel))
        If Not value.Item.CanBuy Then
            Return
        End If
        Context.Model.CurrentPrice = value.Item
        SetState(GameState.Buy)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IAvatarTraderPriceModel))
        Context.Model.CurrentPrice = Nothing
        Return Context.Model.State.Avatar.Trader.Prices.Select(Function(x) (x.Name, x)).ToList
    End Function
End Class

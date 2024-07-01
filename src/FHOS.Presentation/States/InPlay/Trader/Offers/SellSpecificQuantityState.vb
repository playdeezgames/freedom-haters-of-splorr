Imports SPLORR.UI

Friend Class SellSpecificQuantityState
    Inherits BasePickerState(Of Model.IUniverseModel, Integer)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "<placeholder>",
            context.ChooseOrCancel,
            GameState.SellQuantity)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As Integer))
        Context.Model.Ephemerals.SellQuantity = value.Item
        SetState(GameState.Sell)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As Integer))
        With Context.Model.Ephemerals.CurrentOffer
            _pageSize = If(.Quantity >= 20, .Quantity \ 10, Nothing)
            HeaderText = $"Sell How Many { .Name}?"
            Return Enumerable.Range(1, .Quantity).Select(Function(x) ($"{x}", x)).ToList
        End With
    End Function
End Class

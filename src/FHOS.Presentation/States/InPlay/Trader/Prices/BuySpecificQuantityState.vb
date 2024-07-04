Imports FHOS.Model
Imports SPLORR.UI

Friend Class BuySpecificQuantityState
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
            GameState.BuyQuantity)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As Integer))
        Context.Model.Ephemerals.BuyQuantity = value.Item
        SetState(GameState.BuyConfirm)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As Integer))
        With Context.Model.Ephemerals.CurrentPrice
            PageSize = If(.MaximumQuantity >= 20, 20, Nothing)
            HeaderText = $"Buy How Many { .Name}?"
            Return Enumerable.Range(1, .MaximumQuantity).Select(Function(x) ($"{x}(@{ .TotalPrice(x)})", x)).ToList
        End With
    End Function
    Public Overrides Sub Render(displayBuffer As IPixelSink)
        MyBase.Render(displayBuffer)
        Dim font = Context.Font(UIFont)
        Dim jools = Context.Model.State.Avatar.Jools
        font.WriteCenteredText(
            displayBuffer,
            (Context.ViewCenter.X, font.Height),
            $"Jools: {jools}",
            Hues.Blue)
    End Sub
End Class

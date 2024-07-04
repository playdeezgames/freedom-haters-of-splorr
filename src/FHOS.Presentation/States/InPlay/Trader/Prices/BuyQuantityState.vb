Imports FHOS.Model
Imports SPLORR.UI

Friend Class BuyQuantityState
    Inherits BasePickerState(Of Model.IUniverseModel, String)
    Const QuantityOne = "One"
    Const QuantityMaximum = "Maximum"
    Const QuantitySpecificNumber = "Specific Number"

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
            GameState.Offers)
    End Sub

    Public Overrides Sub OnStart()
        If Context.Model.Ephemerals.CurrentPrice.MaximumQuantity = 1 Then
            SetState(GameState.Buy)
            Return
        End If
        MyBase.OnStart()
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case QuantityMaximum
                Context.Model.Ephemerals.BuyQuantity = Context.Model.Ephemerals.CurrentPrice.MaximumQuantity
                SetState(GameState.BuyConfirm)
            Case QuantityOne
                Context.Model.Ephemerals.BuyQuantity = 1
                SetState(GameState.BuyConfirm)
            Case Else
                SetState(GameState.BuySpecificQuantity)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        With Context.Model.Ephemerals.CurrentPrice
            HeaderText = $"Buy How Many { .Name}?"
            Dim result As New List(Of (Text As String, Item As String)) From
            {
                ($"{QuantityOne}(1)", QuantityOne),
                ($"{QuantityMaximum}({ .MaximumQuantity})", QuantityMaximum)
            }
            If .MaximumQuantity > 2 Then
                result.Add(($"{QuantitySpecificNumber}...", QuantitySpecificNumber))
            End If
            Return result
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

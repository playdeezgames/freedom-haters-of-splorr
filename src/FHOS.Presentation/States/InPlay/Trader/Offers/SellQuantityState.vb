Imports SPLORR.UI

Friend Class SellQuantityState
    Inherits BasePickerState(Of Model.IUniverseModel, String)
    Const QuantityOne = "One"
    Const QuantityAll = "All"
    Const QuantityHalf = "Half"
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
        If Context.Model.Ephemerals.CurrentOffer.Quantity = 1 Then
            SetState(GameState.Sell)
            Return
        End If
        MyBase.OnStart()
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case QuantityAll
                Context.Model.Ephemerals.SellQuantity = Context.Model.Ephemerals.CurrentOffer.Quantity
                SetState(GameState.Sell)
            Case QuantityOne
                Context.Model.Ephemerals.SellQuantity = 1
                SetState(GameState.Sell)
            Case QuantityHalf
                Context.Model.Ephemerals.SellQuantity = Context.Model.Ephemerals.CurrentOffer.Quantity \ 2
                SetState(GameState.Sell)
            Case Else
                SetState(GameState.SellSpecificQuantity)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        With Context.Model.Ephemerals.CurrentOffer
            HeaderText = $"Sell How Many { .Name}?"
            Dim result As New List(Of (Text As String, Item As String)) From
            {
                ($"{QuantityOne}(1)", QuantityOne),
                ($"{QuantityAll}({ .Quantity})", QuantityAll)
            }
            If .Quantity > 3 Then
                result.Add(($"{QuantityHalf}({ .Quantity \ 2})", QuantityHalf))
            End If
            If .Quantity > 2 Then
                result.Add(($"{QuantitySpecificNumber}...", QuantitySpecificNumber))
            End If
            Return result
        End With
    End Function
End Class

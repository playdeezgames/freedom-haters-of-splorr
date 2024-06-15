Imports FHOS.Model
Imports SPLORR.UI

Friend Class TraderState
    Inherits BasePickerState(Of IUniverseModel, String)
    Const BuyText = "Buy"
    Const SellText = "Sell"
    Const LeaveText = "Leave"

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context, "<placeholder>",
            context.ChooseOrCancel,
            GameState.LeaveTrader)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case BuyText
                SetState(GameState.Prices)
            Case SellText
                SetState(GameState.Offers)
            Case LeaveText
                SetState(GameState.LeaveTrader)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        With Model.State.Avatar.Trader
            HeaderText = .Trader.Name
            Dim result As New List(Of (Text As String, Item As String)) From
                {
                    (LeaveText, LeaveText)
                }
            If .HasPrices Then
                result.Add((BuyText, BuyText))
            End If
            If .HasOffers Then
                result.Add((SellText, SellText))
            End If
            Return result
        End With
    End Function
End Class

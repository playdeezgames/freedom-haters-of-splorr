Imports FHOS.Model
Imports SPLORR.UI

Friend Class TraderState
    Inherits BasePickerState(Of IUniverseModel, String)
    Const BuyText = "Buy"
    Const SellText = "Sell"

    Public Sub New(parent As IGameController, setState As Action(Of String, Boolean), context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context, "<placeholder>",
            context.ChooseOrCancel,
            GameState.LeaveTrader)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Throw New NotImplementedException()
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Return {
            (BuyText, BuyText),
            (SellText, SellText)}.ToList
    End Function
End Class

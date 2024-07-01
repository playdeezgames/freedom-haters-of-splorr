Imports SPLORR.UI

Friend Class BuyState
    Inherits BasePickerState(Of Model.IUniverseModel, Integer)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Buy",
            context.ChooseOrCancel,
            GameState.Prices)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As Integer))
        Throw New NotImplementedException()
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As Integer))
        Throw New NotImplementedException()
    End Function

    Public Overrides Sub OnStart()
        If Context.Model.Ephemerals.CurrentPrice.MaximumQuantity < 1 Then
            SetState(GameState.Prices)
        End If
        Context.Model.Ephemerals.CurrentPrice.Buy(1)
        SetState(GameState.Prices)
        'MyBase.OnStart()
    End Sub
End Class

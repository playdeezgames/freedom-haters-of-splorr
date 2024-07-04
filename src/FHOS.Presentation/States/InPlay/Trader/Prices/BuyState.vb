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
        With Context.Model.Ephemerals
            If .CurrentPrice.MaximumQuantity < 1 Then
                SetState(GameState.Prices)
            End If
            .CurrentPrice.Buy(.BuyQuantity)
            SetState(GameState.Prices)
        End With
    End Sub
End Class

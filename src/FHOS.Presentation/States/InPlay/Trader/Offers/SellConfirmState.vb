Imports SPLORR.UI

Friend Class SellConfirmState
    Inherits BaseConfirmState(Of Model.IUniverseModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "<placeholder>",
            GameState.Sell,
            GameState.SellQuantity)
    End Sub

    Protected Overrides Sub OnConfirm()
        'do nothing
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (String, Boolean))
        With Context.Model.Ephemerals
            HeaderText = $"Sell { .SellQuantity} { .CurrentOffer.Name} for { .CurrentOffer.JoolsOffered(.SellQuantity)} Jools?"
        End With
        Return MyBase.InitializeMenuItems()
    End Function
End Class

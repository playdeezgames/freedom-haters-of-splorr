Imports FHOS.Model
Imports SPLORR.UI

Friend Class BuyConfirmState
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
            GameState.Buy,
            GameState.BuyQuantity)
    End Sub

    Protected Overrides Sub OnConfirm()
        'do nothing
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (String, Boolean))
        With Context.Model.Ephemerals
            HeaderText = $"Buy { .BuyQuantity} { .CurrentPrice.Name} for { .CurrentPrice.TotalPrice(.BuyQuantity)} Jools?"
        End With
        Return MyBase.InitializeMenuItems()
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

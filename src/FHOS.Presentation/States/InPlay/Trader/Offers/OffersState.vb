Imports FHOS.Model
Imports SPLORR.UI

Friend Class OffersState
    Inherits BasePickerState(Of IUniverseModel, IAvatarTraderOfferModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Offers",
            context.ChooseOrCancel,
            GameState.Trader)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IAvatarTraderOfferModel))
        Context.Model.Ephemerals.CurrentOffer = value.Item
        SetState(GameState.SellQuantity)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IAvatarTraderOfferModel))
        Context.Model.Ephemerals.CurrentOffer = Nothing
        Dim menuItems = Context.Model.State.Avatar.Trader.Offers.Select(Function(x) ($"{x.NameAndQuantity}@{x.JoolsOffered(1)}", x))
        Return menuItems.ToList
    End Function
    Public Overrides Sub OnStart()
        If Not Context.Model.State.Avatar.Trader.Offers.Any(Function(x) x.Quantity > 0) Then
            SetState(GameState.Trader)
        End If
        MyBase.OnStart()
    End Sub
    Public Overrides Sub Render(displayBuffer As IPixelSink)
        MyBase.Render(displayBuffer)
        Dim font = Context.Font(UIFont)
        font.WriteCenteredText(displayBuffer, (Context.ViewCenter.X, font.Height), $"Jools: {Context.Model.State.Avatar.Jools}", DarkGray)
    End Sub
End Class

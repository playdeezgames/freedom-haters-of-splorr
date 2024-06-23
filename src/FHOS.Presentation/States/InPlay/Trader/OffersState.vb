Imports FHOS.Model
Imports SPLORR.UI

Friend Class OffersState
    Inherits BasePickerState(Of IUniverseModel, IAvatarTraderOfferModel)

    Friend Shared CurrentOffer As IAvatarTraderOfferModel = Nothing

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
        CurrentOffer = value.Item
        SetState(GameState.Sell)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IAvatarTraderOfferModel))
        CurrentOffer = Nothing
        Dim menuItems = Context.Model.State.Avatar.Trader.Offers.Where(Function(x) x.Quantity > 0).Select(Function(x) (x.Name, x))
        Return menuItems.ToList
    End Function
    Public Overrides Sub OnStart()
        If Not Context.Model.State.Avatar.Trader.Offers.Any(Function(x) x.Quantity > 0) Then
            SetState(GameState.Trader)
        End If
    End Sub
End Class

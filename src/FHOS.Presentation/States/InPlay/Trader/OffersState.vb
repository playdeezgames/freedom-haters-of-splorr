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
        Throw New NotImplementedException()
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IAvatarTraderOfferModel))
        Return Context.Model.State.Avatar.Trader.Offers.Select(Function(x) (x.Name, x)).ToList
    End Function
End Class

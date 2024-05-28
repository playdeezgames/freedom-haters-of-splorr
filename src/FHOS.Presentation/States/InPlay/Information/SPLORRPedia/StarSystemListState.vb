Imports FHOS.Model
Imports SPLORR.UI

Friend Class StarSystemListState
    Inherits BasePickerState(Of IUniverseModel, IGroupModel)
    Friend Shared SelectedStarSystem As IGroupModel

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Star Systems",
            context.ChooseOrCancel,
            Nothing)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IGroupModel))
        SelectedStarSystem = value.Item
        PushState(GameState.StarSystemDetails)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IGroupModel))
        Return Context.Model.Pedia.StarSystemList.Select(Function(x) (x.Name, x)).ToList
    End Function
End Class

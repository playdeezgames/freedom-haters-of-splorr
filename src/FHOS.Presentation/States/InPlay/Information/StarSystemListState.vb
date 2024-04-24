Imports FHOS.Model
Imports SPLORR.UI

Friend Class StarSystemListState
    Inherits BasePickerState(Of Model.IUniverseModel, IStarSystemModel)
    Friend Shared Property SelectedStarSystem As IStarSystemModel

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Known Star Systems",
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            GameState.ActionMenu)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IStarSystemModel))
        SelectedStarSystem = value.Item
        SetState(GameState.StarSystemDetails)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IStarSystemModel))
        Return Context.Model.Avatar.StarSystemList.ToList
    End Function
End Class

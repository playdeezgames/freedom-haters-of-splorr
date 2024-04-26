Imports FHOS.Model
Imports SPLORR.UI

Friend Class StarVicinityListState
    Inherits BasePickerState(Of Model.IUniverseModel, IStarVicinityModel)
    Friend Shared Property SelectedStarVicinity As IStarVicinityModel

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Known Star Vicinities",
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            GameState.ActionMenu)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IStarVicinityModel))
        SelectedStarVicinity = value.Item
        SetState(GameState.StarVicinityDetails)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IStarVicinityModel))
        Return Context.Model.Avatar.StarVicinityList.ToList
    End Function
End Class

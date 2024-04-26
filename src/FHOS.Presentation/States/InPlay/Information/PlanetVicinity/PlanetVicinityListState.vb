Imports FHOS.Model
Imports SPLORR.UI

Friend Class PlanetVicinityListState
    Inherits BasePickerState(Of IUniverseModel, IPlanetVicinityModel)
    Friend Shared Property SelectedPlanetVicinity As IPlanetVicinityModel

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Known Planet Vicinities",
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            GameState.ActionMenu)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IPlanetVicinityModel))
        SelectedPlanetVicinity = value.Item
        SetState(GameState.PlanetVicinityDetails)
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IPlanetVicinityModel))
        Return Context.Model.Avatar.PlanetVicinityList.ToList
    End Function
End Class

Imports FHOS.Model
Imports SPLORR.UI

Friend Class PlanetListState
    Inherits BasePickerState(Of IUniverseModel, IPlaceModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Planets",
            context.ControlsText(bButton:="Cancel"),
            GameState.SPLORRPedia,
            pageSize:=20)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IPlaceModel))
        'TODO: go to details?
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IPlaceModel))
        Return Context.Model.Planets.ToList
    End Function
End Class

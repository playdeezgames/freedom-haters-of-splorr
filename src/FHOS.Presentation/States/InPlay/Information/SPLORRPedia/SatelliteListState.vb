Imports FHOS.Model
Imports SPLORR.UI

Friend Class SatelliteListState
    Inherits BasePickerState(Of IUniverseModel, IPlaceModel)

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Satellites",
            context.ControlsText(bButton:="Cancel"),
            GameState.SPLORRPedia,
            pageSize:=20)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As IPlaceModel))
        'TODO: go to details?
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As IPlaceModel))
        Return Context.Model.Pedia.Satellites.ToList
    End Function
End Class

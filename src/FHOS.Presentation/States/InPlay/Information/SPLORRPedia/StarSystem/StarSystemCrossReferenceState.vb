Imports FHOS.Model
Imports SPLORR.UI

Friend Class StarSystemCrossReferenceState
    Inherits BasePickerState(Of IUniverseModel, String)
    Const PlanetsText = "Planets..."
    Const SatellitesText = "Satellites..."

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Go To...",
            context.ChooseOrCancel,
            Nothing)
    End Sub

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        Select Case value.Item
            Case PlanetsText
                SetState(GameState.StarSystemPlanetList)
            Case SatellitesText
                SetState(GameState.StarSystemSatelliteList)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Return {
                (PlanetsText, PlanetsText),
                (SatellitesText, SatellitesText)
            }.ToList
    End Function
End Class

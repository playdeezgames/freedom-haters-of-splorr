Imports FHOS.Model
Imports SPLORR.UI

Friend Class SatelliteCrossReferenceState
    Inherits BasePickerState(Of IUniverseModel, String)
    Const PlanetText = "Planet..."
    Const StarSystemText = "Star System..."

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
            Case StarSystemText
                StarSystemListState.SelectedStarSystem.Push(SatelliteListState.SelectedSatellite.Peek.Parents.ParentStarSystem)
                SetState(GameState.StarSystemDetails)
            Case PlanetText
                PlanetListState.SelectedPlanet.Push(SatelliteListState.SelectedSatellite.Peek.Parents.ParentPlanet)
                SetState(GameState.PlanetDetails)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Return {
                (PlanetText, PlanetText),
                (StarSystemText, StarSystemText)
            }.ToList
    End Function
End Class

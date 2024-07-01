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
                Context.Model.SelectedStarSystem.Push(Context.Model.SelectedSatellite.Peek.Parents.StarSystem)
                SetState(GameState.StarSystemDetails)
            Case PlanetText
                Context.Model.SelectedPlanet.Push(Context.Model.SelectedSatellite.Peek.Parents.Planet)
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

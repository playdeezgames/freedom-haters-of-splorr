Imports FHOS.Model
Imports SPLORR.UI

Friend Class PlanetCrossReferenceState
    Inherits BasePickerState(Of IUniverseModel, String)
    Const FactionText = "Faction..."
    Const StarSystemText = "Star System..."
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
            Case StarSystemText
                Context.Model.Ephemerals.SelectedStarSystem.Push(Context.Model.Ephemerals.SelectedPlanet.Peek.Parents.StarSystem)
                SetState(GameState.StarSystemDetails)
            Case SatellitesText
                SetState(GameState.PlanetSatelliteList)
            Case FactionText
                Context.Model.Ephemerals.SelectedFaction.Push(Context.Model.Ephemerals.SelectedPlanet.Peek.Parents.Faction)
                SetState(GameState.FactionDetails)
        End Select
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Dim items = {
                (FactionText, FactionText),
                (StarSystemText, StarSystemText)
            }.ToList
        If Context.Model.Ephemerals.SelectedPlanet.Peek.Children.ChildSatellites.Any Then
            items.Add((SatellitesText, SatellitesText))
        End If
        Return items
    End Function
End Class

Imports SPLORR.UI
Imports FHOS.Model

Friend Class ActionMenuState
    Inherits BasePickerState(Of Model.IUniverseModel, String)
    Private Const DistressSignalText = "Distress Signal"
    Private Const KnownPlacesText = "Known Places..."

    Public Sub New(
                  parent As IGameController,
                  setState As Action(Of String, Boolean),
                  context As IUIContext(Of Model.IUniverseModel))
        MyBase.New(
            parent,
            setState,
            context,
            "Action Menu",
            context.ControlsText(aButton:="Choose", bButton:="Cancel"),
            BoilerplateState.Neutral)
    End Sub

    Private ReadOnly actionMap As IReadOnlyDictionary(Of String, String) =
        New Dictionary(Of String, String) From
        {
            {GoBackText, BoilerplateState.Neutral},
            {VerbTypes.EnterStarSystem, GameState.EnterStarSystem},
            {ApproachPlanetVicinity, GameState.ApproachPlanet},
            {ApproachStarVicinity, GameState.ApproachStar},
            {VerbTypes.RefillOxygen, GameState.RefillOxygen},
            {VerbTypes.Refuel, GameState.Refuel},
            {DistressSignalText, GameState.SignalDistress},
            {KnownPlacesText, GameState.KnownPlaces}
        }

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        SetState(actionMap(value.Item))
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Dim result As New List(Of (Text As String, Item As String)) From
            {
                (GoBackText, GoBackText)
            }
        With Context.Model.Avatar
            If .KnowsPlaces Then
                result.Add((KnownPlacesText, KnownPlacesText))
            End If
            For Each verb In .Place.Verbs
                result.Add((verb, verb))
            Next
            If Not .CanMove Then
                result.Add((DistressSignalText, DistressSignalText))
            End If
        End With
        Return result
    End Function
End Class

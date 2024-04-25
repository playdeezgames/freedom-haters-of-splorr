Imports SPLORR.UI

Friend Class ActionMenuState
    Inherits BasePickerState(Of Model.IUniverseModel, String)
    Private Const EnterStarSystemText = "Enter Star System"
    Private Const ApproachPlanetText = "Approach Planet"
    Private Const ApproachStarText = "Approach Star"
    Private Const RefillOxygenText = "Refill Oxygen"
    Private Const RefuelText = "Refuel"
    Private Const StarSystemListText = "Known Star Systems..."
    Private Const DistressSignalText = "Distress Signal"

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
            {EnterStarSystemText, GameState.EnterStarSystem},
            {ApproachPlanetText, GameState.ApproachPlanet},
            {ApproachStarText, GameState.ApproachStar},
            {RefillOxygenText, GameState.RefillOxygen},
            {RefuelText, GameState.Refuel},
            {StarSystemListText, GameState.StarSystemList},
            {DistressSignalText, GameState.SignalDistress}
        }

    Protected Overrides Sub OnActivateMenuItem(value As (Text As String, Item As String))
        SetState(actionMap(value.Item))
    End Sub

    Protected Overrides Function InitializeMenuItems() As List(Of (Text As String, Item As String))
        Dim result As New List(Of (Text As String, Item As String)) From
            {
                (GoBackText, GoBackText)
            }
        If Context.Model.Avatar.KnowsStarSystems Then
            result.Add((StarSystemListText, StarSystemListText))
        End If
        If Context.Model.Avatar.StarSystem.CanEnter Then
            result.Add((EnterStarSystemText, EnterStarSystemText))
        End If
        If Context.Model.Avatar.StarVicinity.CanApproach Then
            result.Add((ApproachStarText, ApproachStarText))
        End If
        If Context.Model.Avatar.PlanetVicinity.CanApproach Then
            result.Add((ApproachPlanetText, ApproachPlanetText))
        End If
        If Context.Model.Avatar.Planet.CanRefillOxygen Then
            result.Add((RefillOxygenText, RefillOxygenText))
        End If
        If Context.Model.Avatar.Star.CanRefillFuel Then
            result.Add((RefuelText, RefuelText))
        End If
        If Not Context.Model.Avatar.CanMove Then
            result.Add((DistressSignalText, DistressSignalText))
        End If
        Return result
    End Function
End Class

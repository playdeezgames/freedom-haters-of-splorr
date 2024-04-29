Imports FHOS.Model
Imports FHOS.Persistence
Imports SPLORR.UI

Public Class FHOSController
    Inherits BaseGameController(Of IUniverseModel)

    Public Sub New(settings As ISettings, context As IUIContext(Of IUniverseModel))
        MyBase.New(settings, context)
        SetState(BoilerplateState.Neutral, New NeutralState(Me, AddressOf SetCurrentState, context))

        CreateEmbarkationStates(context)

        SetState(GameState.Navigation, New NavigationState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Scanner, New ScannerState(Me, AddressOf SetCurrentState, context))

        CreateActionStates(context)

        CreateExplorationStates(context)

        SetState(GameState.Tutorial, New TutorialState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Message, New MessageState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.GameOver, New GameOverState(Me, AddressOf SetCurrentState, context))

        SetState(GameState.ActionMenu, New ActionMenuState(Me, AddressOf SetCurrentState, context))

        SetCurrentState(BoilerplateState.Splash, True)
    End Sub

    Private Sub CreateExplorationStates(context As IUIContext(Of IUniverseModel))
        SetState(GameState.StarSystemList, New StarSystemListState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.StarSystemDetails, New StarSystemDetailsState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.StarVicinityList, New StarVicinityListState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.StarVicinityDetails, New StarVicinityDetailsState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.PlanetVicinityList, New PlanetVicinityListState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.PlanetVicinityDetails, New PlanetVicinityDetailsState(Me, AddressOf SetCurrentState, context))
    End Sub

    Private Sub CreateActionStates(context As IUIContext(Of IUniverseModel))
        SetState(GameState.MoveUp, New MoveState(Me, AddressOf SetCurrentState, context, Facing.Up))
        SetState(GameState.MoveDown, New MoveState(Me, AddressOf SetCurrentState, context, Facing.Down))
        SetState(GameState.MoveLeft, New MoveState(Me, AddressOf SetCurrentState, context, Facing.Left))
        SetState(GameState.MoveRight, New MoveState(Me, AddressOf SetCurrentState, context, Facing.Right))
        SetState(GameState.RefillOxygen, New RefillOxygenState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Refuel, New RefuelState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.SignalDistress, New SignalDistressState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.EnterStarSystem, New EnterStarSystemState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.ApproachPlanet, New ApproachPlanetState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.ApproachStar, New ApproachStarState(Me, AddressOf SetCurrentState, context))
    End Sub

    Private Sub CreateEmbarkationStates(context As IUIContext(Of IUniverseModel))
        SetState(BoilerplateState.Embark, New EmbarkState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.ChangeGalacticAge, New ChangeGalacticAgeState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.ChangeGalacticDensity, New ChangeGalacticDensityState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.ChangeStartingWealthLevel, New ChangeStartingWealthLevelState(Me, AddressOf SetCurrentState, context))
    End Sub
End Class

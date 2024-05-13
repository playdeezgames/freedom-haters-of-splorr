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

        SetState(GameState.Interaction, New InteractionState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.LeaveInteraction, New LeaveInteractionState(Me, AddressOf SetCurrentState, context))

        SetState(GameState.SPLORRPedia, New SPLORRPediaState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.FactionList, New FactionListState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.FactionDetails, New FactionDetailState(Me, AddressOf SetCurrentState, context))

        SetState(GameState.ActionMenu, New ActionMenuState(Me, AddressOf SetCurrentState, context))

        SetCurrentState(BoilerplateState.Splash, True)
    End Sub

    Private Sub CreateExplorationStates(context As IUIContext(Of IUniverseModel))
        SetState(GameState.KnownPlaces, New KnownPlacesState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.StarSystemList,
                 New KnownPlaceListState(
                     Me,
                     AddressOf SetCurrentState, context,
                     "Known Star Systems",
                     GameState.KnownPlaces,
                     PlaceTypes.StarSystem,
                     GameState.StarSystemDetails))
        SetState(GameState.StarSystemDetails, New StarSystemDetailsState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.PlanetVicinityList,
                 New KnownPlaceListState(
                     Me,
                     AddressOf SetCurrentState, context,
                     "Known Planet Vicinities",
                     GameState.KnownPlaces,
                     PlaceTypes.PlanetVicinity,
                     GameState.PlanetVicinityDetails))
        SetState(GameState.PlanetVicinityDetails, New PlanetVicinityDetailsState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.StarVicinityList,
                 New KnownPlaceListState(
                     Me,
                     AddressOf SetCurrentState, context,
                     "Known Star Vicinities",
                     GameState.KnownPlaces,
                     PlaceTypes.StarVicinity,
                     GameState.StarVicinityDetails))
        SetState(GameState.StarVicinityDetails, New StarVicinityDetailsState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.PlanetList,
                 New KnownPlaceListState(
                     Me,
                     AddressOf SetCurrentState, context,
                     "Known Planets",
                     GameState.KnownPlaces,
                     PlaceTypes.Planet,
                     GameState.PlanetDetails))
        SetState(GameState.PlanetDetails, New PlanetDetailsState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.SatelliteList,
                 New KnownPlaceListState(
                     Me,
                     AddressOf SetCurrentState, context,
                     "Known Satellites",
                     GameState.KnownPlaces,
                     PlaceTypes.Satellite,
                     GameState.SatelliteDetails))
        SetState(GameState.SatelliteDetails, New SatelliteDetailsState(Me, AddressOf SetCurrentState, context))
    End Sub

    Private ReadOnly doVerbStates As IReadOnlyDictionary(Of String, String) =
        New Dictionary(Of String, String) From
        {
            {GameState.EnterStarSystem, VerbTypes.EnterStarSystem},
            {GameState.EnterWormhole, VerbTypes.EnterWormhole},
            {GameState.ApproachStar, VerbTypes.ApproachStarVicinity},
            {GameState.ApproachPlanet, VerbTypes.ApproachPlanetVicinity},
            {GameState.RefillOxygen, VerbTypes.RefillOxygen},
            {GameState.Refuel, VerbTypes.Refuel},
            {GameState.EnterOrbit, VerbTypes.EnterOrbit},
            {GameState.MoveDown, VerbTypes.MoveDown},
            {GameState.MoveUp, VerbTypes.MoveUp},
            {GameState.MoveLeft, VerbTypes.MoveLeft},
            {GameState.MoveRight, VerbTypes.MoveRight},
            {GameState.SignalDistress, VerbTypes.DistressSignal}
        }

    Private Sub CreateActionStates(context As IUIContext(Of IUniverseModel))
        SetState(GameState.Status, New StatusState(Me, AddressOf SetCurrentState, context))

        For Each state In doVerbStates
            SetState(state.Key, New DoVerbState(Me, AddressOf SetCurrentState, context, state.Value))
        Next
    End Sub

    Private Sub CreateEmbarkationStates(context As IUIContext(Of IUniverseModel))
        SetState(BoilerplateState.Embark, New EmbarkState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.ChangeGalacticAge, New ChangeGalacticAgeState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.ChangeGalacticDensity, New ChangeGalacticDensityState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.ChangeStartingWealthLevel, New ChangeStartingWealthLevelState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.ChangeFactionCount, New ChangeFactionCountState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Generate, New GenerateState(Me, AddressOf SetCurrentState, context))
    End Sub
End Class

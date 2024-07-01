Imports FHOS.Model
Imports FHOS.Persistence
Imports SPLORR.UI

Public Class FHOSController
    Inherits BaseGameController(Of IUniverseModel)

    Public Sub New(settings As ISettings, context As IUIContext(Of IUniverseModel))
        MyBase.New(settings, context)
        SetState(GameState.Neutral, New NeutralState(Me, AddressOf SetCurrentState, context))

        CreateEmbarkationStates(context)

        SetState(GameState.Navigation, New NavigationState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Scanner, New ScannerState(Me, AddressOf SetCurrentState, context))

        CreateActionStates(context)

        SetState(GameState.Message, New MessageState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.GameOver, New GameOverState(Me, AddressOf SetCurrentState, context))

        SetState(GameState.Interaction, New InteractionState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.LeaveInteraction, New LeaveInteractionState(Me, AddressOf SetCurrentState, context))

        SetState(GameState.StarGate, New StarGateState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.LeaveStarGate, New LeaveStarGateState(Me, AddressOf SetCurrentState, context))

        SetState(GameState.Trader, New TraderState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.LeaveTrader, New LeaveTraderState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Offers, New OffersState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Sell, New SellState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.SellQuantity, New SellQuantityState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.SellSpecificQuantity, New SellSpecificQuantityState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Prices, New PricesState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Buy, New BuyState(Me, AddressOf SetCurrentState, context))

        CreateSPLORRPediaStates(context)

        SetState(GameState.ActionMenu, New ActionMenuState(Me, AddressOf SetCurrentState, context))

        SetCurrentState(BoilerplateState.Splash, True)
    End Sub

    Private Sub CreateSPLORRPediaStates(context As IUIContext(Of IUniverseModel))
        SetState(GameState.SPLORRPedia, New SPLORRPediaState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.FactionPlanetList, New FactionPlanetListState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.FactionDetails, New FactionDetailState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.FactionList, New FactionListState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.StarSystemList, New StarSystemListState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.StarSystemDetails, New StarSystemDetailState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.StarSystemCrossReference, New StarSystemCrossReferenceState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.StarSystemPlanetList, New StarSystemPlanetListState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.StarSystemSatelliteList, New StarSystemSatelliteListState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.PlanetList, New PlanetListState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.PlanetDetails, New PlanetDetailState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.PlanetCrossReference, New PlanetCrossReferenceState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.PlanetSatelliteList, New PlanetSatelliteListState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.SatelliteList, New SatelliteListState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.SatelliteDetails, New SatelliteDetailState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.SatelliteCrossReference, New SatelliteCrossReferenceState(Me, AddressOf SetCurrentState, context))
    End Sub

    Private ReadOnly doVerbStates As IReadOnlyDictionary(Of String, String) =
        New Dictionary(Of String, String) From
        {
            {GameState.MoveDown, VerbTypes.MoveDown},
            {GameState.MoveUp, VerbTypes.MoveUp},
            {GameState.MoveLeft, VerbTypes.MoveLeft},
            {GameState.MoveRight, VerbTypes.MoveRight},
            {GameState.SignalDistress, VerbTypes.DistressSignal}
        }

    Private Sub CreateActionStates(context As IUIContext(Of IUniverseModel))
        SetState(GameState.Status, New StatusState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.SelectCrewMember, New SelectCrewMemberState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.SelectVessel, New SelectVesselState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Inventory, New InventoryState(Me, AddressOf SetCurrentState, context))

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

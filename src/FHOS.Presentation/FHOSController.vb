Imports FHOS.Model
Imports SPLORR.UI

Public Class FHOSController
    Inherits BaseGameController(Of IWorldModel)

    Public Sub New(settings As ISettings, context As IUIContext(Of IWorldModel))
        MyBase.New(settings, context)
        SetState(BoilerplateState.Embark, New EmbarkState(Me, AddressOf SetCurrentState, context))
        SetState(BoilerplateState.Neutral, New NeutralState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.Navigation, New NavigationState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.MoveUp, New MoveState(Me, AddressOf SetCurrentState, context, 0, (0, -1)))
        SetState(GameState.MoveDown, New MoveState(Me, AddressOf SetCurrentState, context, 2, (0, 1)))
        SetState(GameState.MoveLeft, New MoveState(Me, AddressOf SetCurrentState, context, 3, (-1, 0)))
        SetState(GameState.MoveRight, New MoveState(Me, AddressOf SetCurrentState, context, 1, (1, 0)))
        SetState(GameState.Scanner, New ScannerState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.ChangeGalacticAge, New ChangeGalacticAgeState(Me, AddressOf SetCurrentState, context))
        SetState(GameState.ChangeGalacticDensity, New ChangeGalacticDensityState(Me, AddressOf SetCurrentState, context))
        SetCurrentState(BoilerplateState.Splash, True)
    End Sub
End Class

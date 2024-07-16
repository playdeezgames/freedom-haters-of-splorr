Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class MoveRightState
    Inherits DoVerbState

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState)
        MyBase.New(
            model,
            ui,
            endState,
            VerbTypes.MoveRight)
    End Sub
End Class

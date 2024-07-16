Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class MoveLeftState
    Inherits DoVerbState

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState)
        MyBase.New(
            model,
            ui,
            endState,
            VerbTypes.MoveLeft)
    End Sub
End Class

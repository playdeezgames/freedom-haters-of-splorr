Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class MoveDownState
    Inherits DoVerbState

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState)
        MyBase.New(
            model,
            ui,
            endState,
            VerbTypes.MoveDown)
    End Sub
End Class

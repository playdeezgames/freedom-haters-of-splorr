Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class MoveLeftState
    Inherits DoOperationState

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState)
        MyBase.New(
            model,
            ui,
            endState,
            OperationTypes.MoveLeft)
    End Sub
End Class

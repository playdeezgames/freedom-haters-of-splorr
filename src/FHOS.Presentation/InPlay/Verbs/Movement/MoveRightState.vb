Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class MoveRightState
    Inherits DoOperationState

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState)
        MyBase.New(
            model,
            ui,
            endState,
            OperationTypes.MoveRight)
    End Sub
End Class

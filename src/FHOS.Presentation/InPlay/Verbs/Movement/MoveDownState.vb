Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class MoveDownState
    Inherits DoOperationState

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState)
        MyBase.New(
            model,
            ui,
            endState,
            OperationTypes.MoveDown)
    End Sub
End Class

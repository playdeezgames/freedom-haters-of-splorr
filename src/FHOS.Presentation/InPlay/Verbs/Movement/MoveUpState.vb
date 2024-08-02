Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class MoveUpState
    Inherits DoOperationState
    Implements IState

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState)
        MyBase.New(
            model,
            ui,
            endState,
            OperationTypes.MoveUp)
    End Sub
End Class

Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class DoOperationState
    Inherits BaseState

    Private ReadOnly operationType As String

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState,
                  operationType As String)
        MyBase.New(model, ui, endState)
        Me.operationType = operationType
    End Sub

    Public Overrides Function Run() As IState
        model.State.Avatar.Operations.Perform(operationType)
        Return New NeutralState(model, ui, endState)
    End Function
End Class

Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class DoVerbState
    Inherits BaseState

    Private ReadOnly verbType As String

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState,
                  verbType As String)
        MyBase.New(model, ui, endState)
        Me.verbType = verbType
    End Sub

    Public Overrides Function Run() As IState
        model.State.Avatar.Verbs.Perform(verbType)
        Return New NeutralState(model, ui, endState)
    End Function
End Class

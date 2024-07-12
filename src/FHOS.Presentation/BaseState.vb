Imports FHOS.Model
Imports SPLORR.Presentation

Public MustInherit Class BaseState
    Implements IState
    Protected ReadOnly model As IUniverseModel
    Protected ReadOnly ui As IUIContext
    Protected ReadOnly endState As IState
    Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        Me.model = model
        Me.ui = ui
        Me.endState = endState
    End Sub

    Public MustOverride Function Run() As IState Implements IState.Run
End Class

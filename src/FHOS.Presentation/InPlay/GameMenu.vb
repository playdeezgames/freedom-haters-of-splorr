Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class GameMenu
    Inherits BaseState
    Implements IState

    Private ReadOnly abandonedState As IState

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState,
                  abandonState As IState)
        MyBase.New(model, ui, endState)
        Me.abandonedState = abandonState
    End Sub

    Public Overrides Function Run() As IState
        Throw New NotImplementedException()
    End Function
End Class

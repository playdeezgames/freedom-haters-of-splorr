Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class NeutralState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        If Not model.Generator.Done Then
            Return New GenerateState(model, ui, endState)
        End If
        Return endState
    End Function
End Class

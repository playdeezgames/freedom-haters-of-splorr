Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class EmbarkState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        model.Generator.Start()
        Return New NeutralState(model, ui, endState)
    End Function
End Class

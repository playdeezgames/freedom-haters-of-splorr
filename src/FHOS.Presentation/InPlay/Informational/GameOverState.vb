Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class GameOverState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        If model.State.Avatar.Status.Dead Then
            ui.Message((Mood.Red, "Yer Dead!"))
            Return endState
        End If
        If model.State.Avatar.Status.Bankrupt Then
            ui.Message((Mood.Red, "Yer Bankrupt!"))
            Return endState
        End If
        Throw New NotImplementedException
    End Function
End Class

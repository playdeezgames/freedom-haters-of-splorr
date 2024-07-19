Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ScumLoadState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        If Not model.DoesSlotExist(0) Then
            ui.Message((Mood.Danger, "No Scum Slot!"))
            Return New NeutralState(model, ui, endState)
        End If
        model.LoadGame(0)
        Return New NeutralState(model, ui, endState)
    End Function
End Class

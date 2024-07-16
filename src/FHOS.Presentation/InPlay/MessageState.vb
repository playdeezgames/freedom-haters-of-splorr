Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class MessageState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        With model.State.Messages.Current
            ui.WriteLine((Mood.Orange, .Header))
            ui.WriteLine(.Lines.Select(Function(x) (moodTable(x.Hue), x.Text)).ToArray)
        End With
        ui.Message((Mood.Prompt, Messages.Continue))
        model.State.Messages.Dismiss()
        Return New NeutralState(model, ui, endState)
    End Function
End Class

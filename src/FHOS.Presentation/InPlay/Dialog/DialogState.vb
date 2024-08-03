Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class DialogState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        With model.State.Avatar.State.Dialog
            ui.Clear()
            For Each line In .Lines
                ui.WriteLine((moodTable(line.Hue), line.Text))
            Next
            ui.Choose((Mood.Prompt, String.Empty), .Choices.ToArray)()
            Return New NeutralState(model, ui, endState)
        End With
    End Function
End Class

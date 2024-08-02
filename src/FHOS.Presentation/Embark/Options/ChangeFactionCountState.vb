Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ChangeFactionCountState
    Inherits BaseState
    Implements IState

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState)
        MyBase.New(
            model,
            ui,
            endState)
    End Sub

    Public Overrides Function Run() As IState
        Dim menu As New List(Of (Text As String, Item As Integer)) From
            {
                (Choices.Cancel, 0)
            }
        menu.AddRange(model.Settings.FactionCount.Options)
        Dim answer = ui.Choose(
            (Mood.Prompt, Prompts.FactionCount), menu.ToArray)
        If answer <> 0 Then
            model.Settings.FactionCount.SetFactionCount(answer)
        End If
        Return endState
    End Function
End Class

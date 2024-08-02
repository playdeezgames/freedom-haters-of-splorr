Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ChangeGalacticAgeState
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
        Dim menu As New List(Of (Text As String, Item As String)) From
            {
                (Choices.Cancel, Choices.Cancel)
            }
        menu.AddRange(model.Settings.GalacticAge.Options)
        Dim answer = ui.Choose(
            (Mood.Prompt, Prompts.GalacticAge), menu.ToArray)
        If answer <> Choices.Cancel Then
            model.Settings.GalacticAge.SetAge(answer)
        End If
        Return endState
    End Function
End Class

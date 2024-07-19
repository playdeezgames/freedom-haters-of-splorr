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
        ui.Clear()
        Dim choice = ui.Choose(
                (Mood.Prompt, Prompts.GameMenu),
                Choices.ContinueGame,
                Choices.ScumSave,
                Choices.Save,
                Choices.ScumLoad,
                Choices.AbandonGame)
        Select Case choice
            Case Choices.ContinueGame
                Return endState
            Case Choices.AbandonGame
                If ui.Confirm((Mood.Danger, Confirms.AbandonGame)) Then
                    Return abandonedState
                End If
            Case Choices.ScumSave
                Return New ScumSaveState(model, ui, abandonedState)
            Case Choices.ScumLoad
                Return New ScumLoadState(model, ui, abandonedState)
            Case Choices.Save
                Return New SaveState(model, ui, abandonedState)
            Case Else
                ui.Message((Mood.Warning, $"TODO: {choice}"))
        End Select
        Return Me
    End Function
End Class

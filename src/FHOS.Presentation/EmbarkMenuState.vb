Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class EmbarkMenuState
    Inherits BaseState
    Implements IState

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        ui.Write(
            (Mood.Title, $"Embark Settings:"),
            (Mood.Info, $"Galactic Age: "),
            (Mood.Info, $"Galactic Density: "),
            (Mood.Info, $"Starting Wealth: "),
            (Mood.Info, $"Faction Count: "),
            (Mood.Info, String.Empty))
        Dim choice = ui.Choose(
                (Mood.Prompt, Prompts.EmbarkMenu),
                Choices.Cancel,
                Choices.Go,
                Choices.ChangeGalacticAge,
                Choices.ChangeGalacticDensity,
                Choices.ChangeStartingWealth,
                Choices.ChangeFactionCount)
        Select Case choice
            Case Choices.Cancel
                Return endState
            Case Else
                ui.Message((Mood.Warning, $"TODO: {choice}"))
        End Select
        Return Me
    End Function
End Class

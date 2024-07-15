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
            (Mood.Info, $"Galactic Age: {model.Settings.GalacticAge.CurrentName}"),
            (Mood.Info, $"Galactic Density: {model.Settings.GalacticDensity.CurrentName}"),
            (Mood.Info, $"Starting Wealth: {model.Settings.StartingWealth.CurrentName}"),
            (Mood.Info, $"Faction Count: {model.Settings.FactionCount.CurrentName}"),
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
            Case Choices.ChangeGalacticAge
                Return New ChangeGalacticAgeState(model, ui, Me)
            Case Choices.ChangeGalacticDensity
                Return New ChangeGalacticDensityState(model, ui, Me)
            Case Choices.ChangeStartingWealth
                Return New ChangeStartingWealthState(model, ui, Me)
            Case Choices.ChangeFactionCount
                Return New ChangeFactionCountState(model, ui, Me)
            Case Choices.Go
                Return New EmbarkState(model, ui, endState)
            Case Else
                ui.Message((Mood.Warning, $"TODO: {choice}"))
        End Select
        Return Me
    End Function
End Class

Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class SPLORRPediaState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        Dim choice = ui.Choose(
            (Mood.Prompt, Prompts.SPLORRPedia),
            Choices.Cancel,
            Choices.Factions,
            Choices.StarSystems,
            Choices.Planets,
            Choices.Satellites)
        Select Case choice
            Case Choices.Cancel
                Return New ActionMenuState(model, ui, endState)
            Case Choices.Factions
                Return New FactionsState(model, ui, Me, String.Empty)
            Case Choices.StarSystems
                Return New StarSystemsState(model, ui, Me, String.Empty)
            Case Choices.Planets
                Return New PlanetsState(model, ui, Me, String.Empty)
            Case Choices.Satellites
                Return New SatellitesState(model, ui, Me, String.Empty)
            Case Else
                ui.Message((Mood.Warning, $"TODO: {choice}"))
                Return Me
        End Select
    End Function
End Class

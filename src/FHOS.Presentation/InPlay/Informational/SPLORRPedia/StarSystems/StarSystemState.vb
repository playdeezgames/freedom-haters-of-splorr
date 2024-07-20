Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class StarSystemState
    Inherits BaseState
    Implements IState

    Private ReadOnly group As IGroupModel

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, group As IGroupModel)
        MyBase.New(model, ui, endState)
        Me.group = group
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        With group
            ui.WriteLine((Mood.Title, .Name))
            ui.WriteLine((Mood.Info, $"Type: { .Properties.StarTypeName}"))
            Dim galaxyPosition = .Properties.Position
            ui.WriteLine((Mood.Info, $"Position: ({galaxyPosition.Column},{galaxyPosition.Row})"))
            ui.WriteLine((Mood.Info, $"Planet Count: { .Properties.PlanetCount}"))
            ui.WriteLine((Mood.Info, $"Satellite Count: { .Properties.SatelliteCount}"))
            ui.WriteLine((Mood.Info, String.Empty))
            ui.WriteLine((Mood.Info, $"Factions Present:"))
            For Each presentFaction In .Children.ChildPlanetFactions
                ui.WriteLine((Mood.Info, $" - {presentFaction.Name}"))
            Next
        End With
        Select Case ui.Choose(
            (Mood.Prompt, String.Empty),
            Choices.Cancel,
            Choices.Satellites,
            Choices.Factions,
            Choices.Planets)
            Case Choices.Factions
                Return New StarSystemFactionsState(model, ui, Me, group, String.Empty)
            Case Choices.Planets
                Return New StarSystemPlanetsState(model, ui, Me, group, String.Empty)
            Case Choices.Satellites
                Return New StarSystemSatellitesState(model, ui, Me, group, String.Empty)
            Case Else
                Return endState
        End Select
    End Function
End Class

Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class SatelliteState
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
            ui.WriteLine((Mood.Info, $"Type: { .Properties.SatelliteTypeName}"))
            ui.WriteLine((Mood.Info, $"Tech Level: { .Properties.TechLevel}"))
            ui.WriteLine((Mood.Info, $"Planet: { .Parents.Planet.Name}"))
            ui.WriteLine((Mood.Info, $"Star System: { .Parents.StarSystem.Name}"))
            ui.WriteLine((Mood.Info, $"Faction: { .Parents.Planet.Parents.Faction.Name}"))
        End With
        Select Case ui.Choose(
            (Mood.Prompt, String.Empty),
            Choices.Cancel,
            Choices.Faction,
            Choices.StarSystem,
            Choices.Planet)
            Case Choices.Faction
                Return New FactionState(model, ui, Me, group.Parents.Planet.Parents.Faction)
            Case Choices.StarSystem
                Return New StarSystemState(model, ui, Me, group.Parents.StarSystem)
            Case Choices.Planet
                Return New PlanetState(model, ui, Me, group.Parents.Planet)
            Case Else
                Return endState
        End Select
    End Function
End Class

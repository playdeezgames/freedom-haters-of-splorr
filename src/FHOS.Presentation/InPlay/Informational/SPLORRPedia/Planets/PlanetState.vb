Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class PlanetState
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
            Dim reputation = model.State.Avatar.Bio.Reputation(group)
            If reputation IsNot Nothing Then
                ui.WriteLine((Mood.Info, $"Reputation: { reputation.Value}"))
            End If
            ui.WriteLine((Mood.Info, $"Planet Type: { .Properties.PlanetTypeName}"))
            ui.WriteLine((Mood.Info, $"Tech Level: { .Properties.TechLevel}"))
            ui.WriteLine((Mood.Info, $"Star System: { .Parents.StarSystem.Name}"))
            ui.WriteLine((Mood.Info, $"Satellite Count: { .Properties.SatelliteCount}"))
            ui.WriteLine((Mood.Info, $"Faction: { .Parents.Faction.Name}"))
            ui.WriteLine((Mood.Info, String.Empty))
            ui.WriteLine((Mood.Header, $"Values:"))
            For Each groupValue In .Properties.Values
                ui.WriteLine((Mood.Info, $" - {groupValue.Name}: {groupValue.Description}"))
            Next
        End With
        Select Case ui.Choose(
            (Mood.Prompt, String.Empty),
            Choices.Cancel,
            Choices.Satellites,
            Choices.Faction,
            Choices.StarSystem)
            Case Choices.Faction
                Return New FactionState(model, ui, Me, group.Parents.Faction)
            Case Choices.StarSystem
                Return New StarSystemState(model, ui, Me, group.Parents.StarSystem)
            Case Choices.Satellites
                Return New PlanetSatellitesState(model, ui, Me, group, String.Empty)
            Case Else
                Return endState
        End Select
    End Function
End Class

Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class FactionState
    Inherits BaseState
    Implements IState

    Private ReadOnly group As IGroupModel

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState,
                  group As IGroupModel)
        MyBase.New(
            model,
            ui,
            endState)
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
            ui.WriteLine((Mood.Info, $"Authority: { .Properties.Authority.LevelName}({ .Properties.Authority.Value})"))
            ui.WriteLine((Mood.Info, $"Standards: { .Properties.Standards.LevelName}({ .Properties.Standards.Value})"))
            ui.WriteLine((Mood.Info, $"Conviction: { .Properties.Conviction.LevelName}({ .Properties.Conviction.Value})"))
            ui.WriteLine((Mood.Info, $"Planets: { .Properties.PlanetCount}"))
            ui.WriteLine((Mood.Info, String.Empty))
            ui.WriteLine((Mood.Info, $"Other Faction Relationships:"))
            For Each otherFaction In model.Pedia.Factions.Where(Function(x) x.Name <> .Name)
                ui.WriteLine((Mood.Info, $" - {otherFaction.Name}: { .RelationNameTo(otherFaction)}"))
            Next
            ui.WriteLine((Mood.Info, $"Values:"))
            For Each groupValue In .Properties.Values

            Next
        End With
        Select Case ui.Choose(
            (Mood.Prompt, String.Empty),
            Choices.Cancel,
            Choices.Planets)
            Case Choices.Planets
                Return New FactionPlanetsState(model, ui, Me, group, String.Empty)
            Case Else
                Return endState
        End Select
    End Function
End Class

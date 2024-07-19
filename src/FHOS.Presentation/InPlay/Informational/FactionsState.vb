Imports System.Diagnostics.CodeAnalysis
Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class FactionsState
    Inherits BaseState
    Implements IState

    Private ReadOnly filter As String

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, filter As String)
        MyBase.New(model, ui, endState)
        Me.filter = filter
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        Dim table = model.Pedia.Factions.ToDictionary(Function(x) x.Name, Function(x) x)
        Dim menu As New List(Of (String, String)) From
            {
                (Choices.Cancel, Nothing)
            }
        If Not String.IsNullOrWhiteSpace(filter) Then
            menu.Add(($"Change Filter: `{filter}`", String.Empty))
            menu.AddRange(table.Keys.Where(Function(x) x.ToLower.Contains(filter.ToLower)).Select(Function(x) (x, x)))
        Else
            menu.Add(($"Add Filter...", String.Empty))
            menu.AddRange(table.Keys.Select(Function(x) (x, x)))
        End If
        Dim choice = ui.Choose((Mood.Prompt, "Factions:"), menu.ToArray)
        If choice Is Nothing Then
            Return endState
        End If
        If choice = String.Empty Then
            Return New FactionsState(model, ui, endState, ui.Ask(Of String)((Mood.Prompt, "New Filter:"), String.Empty))
        End If
        Return New FactionState(model, ui, Me, table(choice))
    End Function
End Class

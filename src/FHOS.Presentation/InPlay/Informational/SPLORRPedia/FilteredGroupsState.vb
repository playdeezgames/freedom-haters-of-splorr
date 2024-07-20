Imports FHOS.Model
Imports SPLORR.Presentation

Friend MustInherit Class FilteredGroupsState
    Inherits BaseState

    Private ReadOnly filter As String

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, filter As String)
        MyBase.New(model, ui, endState)
        Me.filter = filter
    End Sub

    Protected MustOverride ReadOnly Property GroupSource As IEnumerable(Of IGroupModel)
    Protected MustOverride ReadOnly Property PromptText As String
    Protected MustOverride Function ApplyFilter(filter As String) As IState
    Protected MustOverride Function ToDetail(group As IGroupModel) As IState

    Public Overrides Function Run() As IState
        ui.Clear()
        Dim table = GroupSource.ToDictionary(Function(x) x.Name, Function(x) x)
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
        Dim choice = ui.Choose((Mood.Prompt, PromptText), menu.ToArray)
        If choice Is Nothing Then
            Return endState
        End If
        If choice = String.Empty Then
            Return ApplyFilter(ui.Ask(Of String)((Mood.Prompt, "New Filter (blank to clear):"), String.Empty))
        End If
        Return ToDetail(table(choice))
    End Function
End Class

Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class StarSystemFactionsState
    Inherits FilteredGroupsState
    Implements IState

    Private ReadOnly group As IGroupModel

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, group As IGroupModel, filter As String)
        MyBase.New(model, ui, endState, filter)
        Me.group = group
    End Sub

    Protected Overrides ReadOnly Property GroupSource As IEnumerable(Of IGroupModel)
        Get
            Return group.Children.ChildPlanetFactions
        End Get
    End Property

    Protected Overrides ReadOnly Property PromptText As String
        Get
            Return $"Factions Present in {group.Name}"
        End Get
    End Property

    Protected Overrides Function ApplyFilter(filter As String) As IState
        Return New StarSystemFactionsState(model, ui, endState, group, filter)
    End Function

    Protected Overrides Function ToDetail(group As IGroupModel) As IState
        Return New StarSystemState(model, ui, Me, group)
    End Function
End Class

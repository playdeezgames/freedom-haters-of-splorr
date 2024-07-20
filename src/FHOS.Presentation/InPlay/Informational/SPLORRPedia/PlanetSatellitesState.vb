Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class PlanetSatellitesState
    Inherits FilteredGroupsState
    Implements IState

    Private ReadOnly group As IGroupModel

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, group As IGroupModel, filter As String)
        MyBase.New(model, ui, endState, filter)
        Me.group = group
    End Sub

    Protected Overrides ReadOnly Property GroupSource As IEnumerable(Of IGroupModel)
        Get
            Return group.Children.ChildSatellites
        End Get
    End Property

    Protected Overrides ReadOnly Property PromptText As String
        Get
            Return $"Satellites in {group.Name}:"
        End Get
    End Property

    Protected Overrides Function ApplyFilter(filter As String) As IState
        Return New PlanetSatellitesState(model, ui, endState, group, filter)
    End Function

    Protected Overrides Function ToDetail(group As IGroupModel) As IState
        Return New SatelliteState(model, ui, Me, group)
    End Function
End Class

Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class StarSystemPlanetsState
    Inherits FilteredModelState(Of IGroupModel)
    Implements IState

    Private ReadOnly group As IGroupModel

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, group As IGroupModel, filter As String)
        MyBase.New(model, ui, endState, filter)
        Me.group = group
    End Sub

    Protected Overrides ReadOnly Property ModelSource As IEnumerable(Of IGroupModel)
        Get
            Return group.Children.ChildPlanets
        End Get
    End Property

    Protected Overrides ReadOnly Property PromptText As String
        Get
            Return $"Planets in {group.Name}:"
        End Get
    End Property

    Protected Overrides Function ApplyFilter(filter As String) As IState
        Return New StarSystemPlanetsState(model, ui, endState, group, filter)
    End Function

    Protected Overrides Function OnSelected(group As IGroupModel) As IState
        Return New PlanetState(model, ui, Me, group)
    End Function

    Protected Overrides Function ToName(model As IGroupModel) As String
        Return model.Name
    End Function
End Class

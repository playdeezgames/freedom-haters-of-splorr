Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class PlanetsState
    Inherits FilteredModelState(Of IGroupModel)
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, filter As String)
        MyBase.New(model, ui, endState, filter)
    End Sub

    Protected Overrides ReadOnly Property ModelSource As IEnumerable(Of IGroupModel)
        Get
            Return model.Pedia.PlanetVicinities
        End Get
    End Property

    Protected Overrides ReadOnly Property PromptText As String
        Get
            Return $"Planets:"
        End Get
    End Property

    Protected Overrides Function ApplyFilter(filter As String) As IState
        Return New PlanetsState(model, ui, endState, filter)
    End Function

    Protected Overrides Function OnSelected(group As IGroupModel) As IState
        Return New PlanetState(model, ui, Me, group)
    End Function

    Protected Overrides Function ToName(model As IGroupModel) As String
        Return model.Name
    End Function
End Class

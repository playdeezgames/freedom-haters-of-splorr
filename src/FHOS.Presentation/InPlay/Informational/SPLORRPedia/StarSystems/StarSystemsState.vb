Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class StarSystemsState
    Inherits FilteredGroupsState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, filter As String)
        MyBase.New(model, ui, endState, filter)
    End Sub

    Protected Overrides ReadOnly Property GroupSource As IEnumerable(Of IGroupModel)
        Get
            Return model.Pedia.StarSystems
        End Get
    End Property

    Protected Overrides ReadOnly Property PromptText As String
        Get
            Return $"Star Systems:"
        End Get
    End Property

    Protected Overrides Function ApplyFilter(filter As String) As IState
        Return New StarSystemsState(model, ui, endState, filter)
    End Function

    Protected Overrides Function ToDetail(group As IGroupModel) As IState
        Return New StarSystemState(model, ui, Me, group)
    End Function
End Class

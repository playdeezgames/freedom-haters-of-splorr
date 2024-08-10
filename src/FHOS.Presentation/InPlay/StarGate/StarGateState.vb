Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class StarGateState
    Inherits FilteredModelState(Of IActorModel)
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, filter As String)
        MyBase.New(model, ui, endState, filter)
    End Sub

    Protected Overrides ReadOnly Property PromptText As String
        Get
            Return "Choose Destination:"
        End Get
    End Property

    Protected Overrides ReadOnly Property ModelSource As IEnumerable(Of IActorModel)
        Get
            Return model.State.Avatar.Yokes.StarGate.AvailableGates.Select(Function(x) x.Item)
        End Get
    End Property

    Protected Overrides Function ApplyFilter(filter As String) As IState
        Return New StarGateState(model, ui, endState, filter)
    End Function

    Protected Overrides Function OnSelected(model As IActorModel) As IState
        Me.model.State.Avatar.Yokes.StarGate.Enter(model)
        Return New NeutralState(Me.model, ui, endState)
    End Function

    Protected Overrides Function ToName(model As IActorModel) As String
        Return model.Name
    End Function

    Protected Overrides Function OnCancel() As IState
        model.State.Avatar.Yokes.StarGate.Leave()
        Return New NeutralState(model, ui, endState)
    End Function
End Class

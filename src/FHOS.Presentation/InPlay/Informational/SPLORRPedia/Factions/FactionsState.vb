﻿Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class FactionsState
    Inherits FilteredModelState(Of IGroupModel)

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, filter As String)
        MyBase.New(model, ui, endState, filter)
    End Sub

    Protected Overrides ReadOnly Property ModelSource As IEnumerable(Of IGroupModel)
        Get
            Return model.Pedia.Factions
        End Get
    End Property

    Protected Overrides ReadOnly Property PromptText As String
        Get
            Return "Factions:"
        End Get
    End Property

    Protected Overrides Function ApplyFilter(filter As String) As IState
        Return New FactionsState(model, ui, endState, filter)
    End Function

    Protected Overrides Function OnSelected(group As IGroupModel) As IState
        Return New FactionState(model, ui, Me, group)
    End Function

    Protected Overrides Function ToName(model As IGroupModel) As String
        Return model.Name
    End Function
End Class

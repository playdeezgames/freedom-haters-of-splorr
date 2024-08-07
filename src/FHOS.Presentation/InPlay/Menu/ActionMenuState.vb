﻿Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ActionMenuState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        Dim menu As New List(Of (String, String)) From
            {
                (Choices.Cancel, Choices.Cancel)
            }
        menu.AddRange(model.State.Avatar.Operations.Available)
        Dim choice = ui.Choose(
            (Mood.Prompt, Prompts.ActionMenu),
            menu.ToArray)
        Select Case choice
            Case Choices.Cancel
                Return New NeutralState(model, ui, endState)
            Case OperationTypes.Status
                Return New StatusState(model, ui, endState)
            Case OperationTypes.Inventory
                Return New InventoryState(model, ui, endState)
            Case OperationTypes.Equipment
                Return New EquipmentState(model, ui, endState)
            Case OperationTypes.SPLORRPedia
                Return New SPLORRPediaState(model, ui, endState)
            Case Else
                Return New DoOperationState(model, ui, endState, choice)
        End Select
    End Function
End Class

Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class InventoryState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        Dim result As New List(Of (String, IAvatarInventoryItemStackModel)) From
            {
                (Choices.Cancel, Nothing)
            }
        result.AddRange(model.State.Avatar.Inventory.ItemStacks.Select(Function(x) ($"{x.ItemTypeName}(x{x.Count})", x)))
        Dim choice = ui.Choose(
            (Mood.Prompt, Prompts.Inventory),
            result.ToArray)
        If choice Is Nothing Then
            Return New StatusState(model, ui, endState)
        End If
        If choice.Items.Count = 1 Then
            Return New ItemInspectState(model, ui, Me, choice.Items.Single)
        End If
        If choice.Substacks.Count = 1 Then
            Return New ItemSubstackInspectState(model, ui, Me, choice.Substacks.First)
        End If
        Return New InventoryActionSelectState(model, ui, endState, choice)
    End Function
End Class

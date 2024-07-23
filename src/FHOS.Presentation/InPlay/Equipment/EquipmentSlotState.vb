Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class EquipmentSlotState
    Inherits BaseState
    Implements IState

    Private ReadOnly slot As IAvatarEquipmentSlotModel

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState,
                  slot As IAvatarEquipmentSlotModel)
        MyBase.New(
            model,
            ui,
            endState)
        Me.slot = slot
    End Sub

    Public Overrides Function Run() As IState
        If slot.Items.Count = 1 Then
            Return New EquipmentSlotItemState(model, ui, endState, slot.Items.Single)
        End If
        ui.Clear()
        Dim menu As New List(Of (String, IItemModel)) From
            {
                (Choices.Cancel, Nothing)
            }
        Dim choice = ui.Choose((Mood.Prompt, Prompts.EquippedItem), menu.ToArray)
        If choice Is Nothing Then
            Return New EquipmentState(model, ui, endState)
        End If
        Return New EquipmentSlotItemState(model, ui, endState, choice)
    End Function
End Class

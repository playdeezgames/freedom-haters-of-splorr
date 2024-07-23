Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class EquipmentState
    Inherits BaseState
    Implements IState

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState)
        MyBase.New(model, ui, endState)
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        Dim menu As New List(Of (String, IAvatarEquipmentSlotModel)) From
            {
                (Choices.Cancel, Nothing)
            }
        menu.AddRange(model.State.Avatar.Equipment.Slots.Select(Function(x) (ToName(x), x)))
        Dim choice = ui.Choose(
            (Mood.Prompt, Prompts.EquipmentSlot),
            menu.ToArray)
        If choice Is Nothing Then
            Return New ActionMenuState(model, ui, endState)
        End If
        Return New EquipmentSlotState(model, ui, endState, choice)
    End Function

    Private Function ToName(model As IAvatarEquipmentSlotModel) As String
        Return $"{model.SlotName}: {String.Join(", ", model.Items.Select(Function(x) x.DisplayName))}"
    End Function
End Class

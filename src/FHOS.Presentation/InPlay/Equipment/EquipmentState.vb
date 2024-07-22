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
        Dim menu As IReadOnlyList(Of (String, IAvatarEquipmentSlotModel)) =
            New List(Of (String, IAvatarEquipmentSlotModel)) From
            {
                (Choices.Cancel, Nothing)
            }
        Dim choice = ui.Choose(
            (Mood.Prompt, Prompts.EquipmentSlot),
            menu.ToArray)
        If choice Is Nothing Then
            Return New ActionMenuState(model, ui, endState)
        End If
        Throw New NotImplementedException
    End Function
End Class

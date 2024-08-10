Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class EquipmentSlotState
    Inherits BaseState
    Implements IState

    Private ReadOnly slot As IAvatarEquipmentSlotModel

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, item As IAvatarEquipmentSlotModel)
        MyBase.New(model, ui, endState)
        Me.slot = item
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        ui.WriteLine(
            (Mood.Title, slot.Item.DisplayName))
        ui.WriteLine(
            slot.Item.Description.Select(Function(x) (Mood.Info, x)).ToArray)
        ui.Message((Mood.Prompt, String.Empty))
        Return New EquipmentState(model, ui, endState)
    End Function
End Class

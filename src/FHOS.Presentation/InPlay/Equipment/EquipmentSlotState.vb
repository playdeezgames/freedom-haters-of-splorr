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
        Return New EquipmentState(model, ui, endState)
    End Function
End Class

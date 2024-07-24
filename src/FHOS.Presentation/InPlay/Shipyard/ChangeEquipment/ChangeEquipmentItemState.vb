Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ChangeEquipmentItemState
    Inherits BaseState
    Implements IState

    Private ReadOnly equipSlot As IActorEquipmentSlotModel

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, equipSlot As IActorEquipmentSlotModel)
        MyBase.New(model, ui, endState)
        Me.equipSlot = equipSlot
    End Sub

    Public Overrides Function Run() As IState
        'TODO: pick the item to replace the item in the equip slot.
        Throw New NotImplementedException()
    End Function
End Class

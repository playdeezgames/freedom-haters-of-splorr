Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ChangeEquipmentItemConfirmState
    Inherits BaseState
    Implements IState

    Private ReadOnly equipSlot As IActorEquipmentSlotModel
    Private ReadOnly item As IItemModel

    Public Sub New(
                  model As IUniverseModel,
                  ui As IUIContext,
                  endState As IState,
                  equipSlot As IActorEquipmentSlotModel,
                  item As IItemModel)
        MyBase.New(
            model,
            ui,
            endState)
        Me.equipSlot = equipSlot
        Me.item = item
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        Dim oldItem = equipSlot.Unequip()
        If oldItem IsNot Nothing Then
            ui.WriteLine((Mood.Info, $"Uninstalled {oldItem.DisplayName} from {equipSlot.SlotName}."))
        End If
        equipSlot.Equip(item)
        ui.WriteLine((Mood.Info, $"Installed {item.DisplayName} on {equipSlot.SlotName}."))
        ui.Message((Mood.Prompt, String.Empty))
        Return New ShipyardState(model, ui, endState)
    End Function
End Class

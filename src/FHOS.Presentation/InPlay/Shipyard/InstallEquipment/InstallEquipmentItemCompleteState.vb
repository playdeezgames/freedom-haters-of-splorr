Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class InstallEquipmentItemCompleteState
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
        Dim fee = item.InstallFee
        equipSlot.Equip(item)
        ui.WriteLine((Mood.Info, $"Installed {item.DisplayName} on {equipSlot.SlotName}."))
        If fee > 0 Then
            ui.WriteLine((Mood.Info, $"Paid Fees: {fee}"))
        End If
        ui.Message((Mood.Prompt, String.Empty))
        Return New ShipyardState(model, ui, endState)
    End Function
End Class

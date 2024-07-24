Imports FHOS.Persistence

Friend Class ActorEquipmentSlotModel
    Implements IActorEquipmentSlotModel

    Private ReadOnly actor As IActor
    Private ReadOnly equipSlot As String

    Public Sub New(actor As IActor, equipSlot As String)
        Me.actor = actor
        Me.equipSlot = equipSlot
    End Sub

    Public ReadOnly Property SlotName As String Implements IActorEquipmentSlotModel.SlotName
        Get
            Return Model.EquipSlots.Descriptors(equipSlot).DisplayName
        End Get
    End Property

    Friend Shared Function FromActorAndSlot(actor As IActor, equipSlot As String) As IActorEquipmentSlotModel
        Return New ActorEquipmentSlotModel(actor, equipSlot)
    End Function
End Class

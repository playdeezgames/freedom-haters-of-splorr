Imports FHOS.Persistence

Friend Class AvatarEquipmentModel
    Implements IAvatarEquipmentModel

    Private ReadOnly actor As IActor

    Public Sub New(actor As IActor)
        Me.actor = actor
    End Sub

    Public ReadOnly Property Slots As IEnumerable(Of IAvatarEquipmentSlotModel) Implements IAvatarEquipmentModel.Slots
        Get
            Return actor.Equipment.AllSlots.Select(Function(x) AvatarEquipmentSlotModel.FromActorAndSlot(actor, x))
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarEquipmentModel
        Return New AvatarEquipmentModel(actor)
    End Function
End Class

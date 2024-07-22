Imports FHOS.Persistence

Friend Class AvatarEquipmentModel
    Implements IAvatarEquipmentModel

    Private ReadOnly actor As IActor

    Public Sub New(actor As IActor)
        Me.actor = actor
    End Sub

    Public ReadOnly Property Slots As IEnumerable(Of IAvatarEquipmentSlotModel) Implements IAvatarEquipmentModel.Slots
        Get
            Dim items = actor.Equipment.All
            Dim itemSlots = items.SelectMany(Function(x) x.Descriptor.EquipSlots.Keys).Distinct
            Return Array.Empty(Of IAvatarEquipmentSlotModel)
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarEquipmentModel
        Return New AvatarEquipmentModel(actor)
    End Function
End Class

Imports FHOS.Persistence

Friend Class AvatarEquipmentModel
    Implements IAvatarEquipmentModel

    Private ReadOnly actor As IActor

    Public Sub New(actor As IActor)
        Me.actor = actor
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarEquipmentModel
        Return New AvatarEquipmentModel(actor)
    End Function
End Class

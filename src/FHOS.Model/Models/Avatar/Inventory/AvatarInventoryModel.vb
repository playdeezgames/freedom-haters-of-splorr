﻿Imports FHOS.Persistence

Friend Class AvatarInventoryModel
    Implements IAvatarInventoryModel

    Private ReadOnly actor As IActor

    Public Sub New(actor As IActor)
        Me.actor = actor
    End Sub

    Public ReadOnly Property ItemStacks As IEnumerable(Of IAvatarInventoryItemStackModel) Implements IAvatarInventoryModel.ItemStacks
        Get
            Return actor.Inventory.Items.GroupBy(Function(x) x.EntityType).Select(Function(x) AvatarInventoryItemStackModel.FromActorAndItemType(actor, x.Key))
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarInventoryModel
        Return New AvatarInventoryModel(actor)
    End Function
End Class

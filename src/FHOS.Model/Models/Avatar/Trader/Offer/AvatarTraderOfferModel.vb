Imports FHOS.Persistence

Friend Class AvatarTraderOfferModel
    Implements IAvatarTraderOfferModel

    Private ReadOnly actor As IActor
    Private ReadOnly itemType As String

    Public Sub New(actor As IActor, itemType As String)
        Me.actor = actor
        Me.itemType = itemType
    End Sub

    Public ReadOnly Property Name As String Implements IAvatarTraderOfferModel.Name
        Get
            Return $"{ItemTypes.Descriptors(itemType).Name}(x{actor.Inventory.ItemCountOfType(itemType)})"
        End Get
    End Property

    Friend Shared Function FromActorOffer(actor As IActor, itemType As String) As IAvatarTraderOfferModel
        Return New AvatarTraderOfferModel(actor, itemType)
    End Function
End Class

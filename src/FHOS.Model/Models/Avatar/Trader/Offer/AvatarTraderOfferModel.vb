Imports FHOS.Persistence

Friend Class AvatarTraderOfferModel
    Implements IAvatarTraderOfferModel

    Private ReadOnly actor As IActor
    Private ReadOnly itemType As String

    Public Sub New(actor As IActor, itemType As String)
        Me.actor = actor
        Me.itemType = itemType
    End Sub

    Public ReadOnly Property NameAndQuantity As String Implements IAvatarTraderOfferModel.NameAndQuantity
        Get
            Return $"{ItemTypes.Descriptors(itemType).Name}(x{actor.Inventory.ItemCountOfType(itemType)})"
        End Get
    End Property

    Public ReadOnly Property Quantity As Integer Implements IAvatarTraderOfferModel.Quantity
        Get
            Return actor.Inventory.ItemCountOfType(itemType)
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IAvatarTraderOfferModel.Name
        Get
            Return ItemTypes.Descriptors(itemType).Name
        End Get
    End Property

    Public Sub Sell(quantity As Integer) Implements IAvatarTraderOfferModel.Sell
        Dim items = actor.Inventory.ItemsOfType(itemType).Take(quantity)
        For Each item In items
            actor.Inventory.Remove(item)
            item.Recycle()
            actor.Yokes.Store(YokeTypes.Wallet).CurrentValue += ItemTypes.Descriptors(itemType).Offer
        Next
    End Sub

    Friend Shared Function FromActorOffer(actor As IActor, itemType As String) As IAvatarTraderOfferModel
        Return New AvatarTraderOfferModel(actor, itemType)
    End Function

    Public Function JoolsOffered(quantity As Integer) As Integer Implements IAvatarTraderOfferModel.JoolsOffered
        Return ItemTypes.Descriptors(itemType).Offer * quantity
    End Function
End Class

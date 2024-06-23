Imports FHOS.Persistence

Friend Class AvatarTraderPriceModel
    Implements IAvatarTraderPriceModel

    Private ReadOnly actor As IActor
    Private ReadOnly itemType As String

    Public Sub New(actor As IActor, itemType As String)
        Me.actor = actor
        Me.itemType = itemType
    End Sub

    Public ReadOnly Property Name As String Implements IAvatarTraderPriceModel.Name
        Get
            Dim descriptor = ItemTypes.Descriptors(itemType)
            Return $"{descriptor.Name}(@{descriptor.Price})"
        End Get
    End Property

    Public ReadOnly Property CanBuy As Boolean Implements IAvatarTraderPriceModel.CanBuy
        Get
            Return False
        End Get
    End Property

    Friend Shared Function FromActorPrice(actor As IActor, itemType As String) As IAvatarTraderPriceModel
        Return New AvatarTraderPriceModel(actor, itemType)
    End Function
End Class

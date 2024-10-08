﻿Imports FHOS.Persistence

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
            Return ItemTypes.Descriptors(itemType).Name
        End Get
    End Property

    Public ReadOnly Property CanBuy As Boolean Implements IAvatarTraderPriceModel.CanBuy
        Get
            Dim price = ItemTypes.Descriptors(itemType).Price
            Return price > 0 AndAlso actor.GetJools >= price
        End Get
    End Property

    Public ReadOnly Property MaximumQuantity As Integer Implements IAvatarTraderPriceModel.MaximumQuantity
        Get
            If Not CanBuy Then
                Return 0
            End If
            Dim price = ItemTypes.Descriptors(itemType).Price
            Return actor.GetJools \ price
        End Get
    End Property

    Public ReadOnly Property UnitPrice As Integer Implements IAvatarTraderPriceModel.UnitPrice
        Get
            Return ItemTypes.Descriptors(itemType).Price
        End Get
    End Property

    Public ReadOnly Property InventoryQuantity As Integer Implements IAvatarTraderPriceModel.InventoryQuantity
        Get
            Return actor.Inventory.ItemCountOfType(itemType)
        End Get
    End Property

    Public Sub Buy(quantity As Integer) Implements IAvatarTraderPriceModel.Buy
        quantity = Math.Min(quantity, MaximumQuantity)
        Dim descriptor = ItemTypes.Descriptors(itemType)
        Dim price = descriptor.Price
        For Each dummy In Enumerable.Range(0, quantity)
            Dim item = descriptor.CreateItem(actor.Universe)
            actor.Inventory.Add(item)
            actor.ChangeJools(-price)
        Next
    End Sub

    Friend Shared Function FromActorPrice(actor As IActor, itemType As String) As IAvatarTraderPriceModel
        Return New AvatarTraderPriceModel(actor, itemType)
    End Function

    Public Function TotalPrice(quantity As Integer) As Integer Implements IAvatarTraderPriceModel.TotalPrice
        Return UnitPrice * quantity
    End Function
End Class

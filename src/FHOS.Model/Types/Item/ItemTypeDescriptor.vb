Imports FHOS.Persistence

Friend MustInherit Class ItemTypeDescriptor
    ReadOnly Property ItemType As String
    ReadOnly Property Name As String
    ReadOnly Property Offer As Integer
    ReadOnly Property Price As Integer
    ReadOnly Property Description As String
    ReadOnly Property CanUse As Boolean
    Private ReadOnly onUse As Action(Of IActor, IItem)
    Private ReadOnly onEquip As Action(Of IActor, IItem)
    Sub New(
           itemType As String,
           name As String,
           description As String,
           Optional offer As Integer = 0,
           Optional price As Integer = 0,
           Optional onUse As Action(Of IActor, IItem) = Nothing,
           Optional onEquip As Action(Of IActor, IItem) = Nothing)
        Me.ItemType = itemType
        Me.Name = name
        Me.Offer = offer
        Me.Price = price
        Me.Description = description
        Me.onUse = onUse
        Me.CanUse = onUse IsNot Nothing
        Me.onEquip = onEquip
    End Sub
    Function CreateItem(universe As IUniverse) As IItem
        Dim item = universe.Factory.CreateItem(ItemType)
        Initialize(item)
        Return item
    End Function
    Protected MustOverride Sub Initialize(item As IItem)

    Friend Sub Use(actor As IActor, item As IItem)
        If CanUse Then
            onUse(actor, item)
        End If
    End Sub
    Friend Sub Equip(actor As IActor, item As IItem)
        onEquip?(actor, item)
    End Sub
End Class

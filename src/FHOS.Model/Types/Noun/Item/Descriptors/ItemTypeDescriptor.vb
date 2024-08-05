Imports FHOS.Data
Imports FHOS.Persistence

Friend MustInherit Class ItemTypeDescriptor
    ReadOnly Property ItemType As String
    ReadOnly Property Name As String
    MustOverride ReadOnly Property Description(item As IItem) As String
    ReadOnly Property EquipSlot As String

    ReadOnly Property Offer As Integer
    ReadOnly Property Price As Integer
    ReadOnly Property UninstallFee As Integer
    ReadOnly Property InstallFee As Integer
    'MustOverride ReadOnly Property Dialogs(item As IItem) As IReadOnlyDictionary(Of String, IDialog)
    ReadOnly Property CanUse As Boolean

    Private ReadOnly onUse As Action(Of IActor, IItem)
    Private ReadOnly onEquip As Action(Of IActor, IItem)
    Private ReadOnly onUnequip As Action(Of IActor, IItem)
    Private ReadOnly toEntityName As Func(Of IItem, String)
    Sub New(
           itemType As String,
           name As String,
           Optional offer As Integer = 0,
           Optional price As Integer = 0,
           Optional onUse As Action(Of IActor, IItem) = Nothing,
           Optional equipSlot As String = Nothing,
           Optional onEquip As Action(Of IActor, IItem) = Nothing,
           Optional onUnequip As Action(Of IActor, IItem) = Nothing,
           Optional installFee As Integer = 0,
           Optional uninstallFee As Integer = 0,
           Optional toEntityName As Func(Of IItem, String) = Nothing)
        Me.ItemType = itemType
        Me.Name = name
        Me.Offer = offer
        Me.Price = price
        Me.onUse = onUse
        Me.CanUse = onUse IsNot Nothing
        Me.EquipSlot = equipSlot
        Me.onEquip = onEquip
        Me.onUnequip = onUnequip
        Me.InstallFee = installFee
        Me.UninstallFee = uninstallFee
        Me.toEntityName = toEntityName
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
    Friend Sub Unequip(actor As IActor, item As IItem)
        onUnequip?(actor, item)
    End Sub
    Friend Function GetEntityName(item As IItem) As String
        Return If(toEntityName IsNot Nothing, toEntityName(item), Name)
    End Function
End Class

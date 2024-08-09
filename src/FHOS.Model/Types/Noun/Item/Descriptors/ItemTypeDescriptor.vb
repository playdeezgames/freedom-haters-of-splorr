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
    MustOverride Function Dialogs(actor As IActor, item As IItem, finalDialog As IDialog) As IReadOnlyDictionary(Of String, IDialog)
    Private ReadOnly legacyOnEquip As Action(Of IActor, IItem)
    Private ReadOnly legacyOnUnequip As Action(Of IActor, IItem)
    Private ReadOnly legacyToEntityName As Func(Of IItem, String)
    Sub New(
           itemType As String,
           name As String,
           Optional offer As Integer = 0,
           Optional price As Integer = 0,
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
        Me.EquipSlot = equipSlot
        Me.legacyOnEquip = onEquip
        Me.legacyOnUnequip = onUnequip
        Me.InstallFee = installFee
        Me.UninstallFee = uninstallFee
        Me.legacyToEntityName = toEntityName
    End Sub
    Function CreateItem(universe As IUniverse) As IItem
        Dim item = universe.Factory.CreateItem(ItemType)
        Initialize(item)
        Return item
    End Function
    Protected MustOverride Sub Initialize(item As IItem)
    Friend Overridable Function Equip(actor As IActor, item As IItem) As Boolean
        Return False
    End Function
    Friend Overridable Function Unequip(actor As IActor, item As IItem) As Boolean
        Return False
    End Function
    Friend Function LegacyEquip(actor As IActor, item As IItem) As Boolean
        If legacyOnEquip IsNot Nothing Then
            legacyOnEquip(actor, item)
            Return True
        End If
        Return False
    End Function
    Friend Function LegacyUnequip(actor As IActor, item As IItem) As Boolean
        If legacyOnUnequip IsNot Nothing Then
            legacyOnUnequip(actor, item)
            Return True
        End If
        Return False
    End Function
    Friend Function GetEntityName(item As IItem) As String
        Return If(legacyToEntityName IsNot Nothing, legacyToEntityName(item), Name)
    End Function
End Class

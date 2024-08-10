Imports FHOS.Data
Imports FHOS.Persistence

Friend MustInherit Class ItemTypeDescriptor
    ReadOnly Property ItemType As String
    ReadOnly Property Name As String
    MustOverride ReadOnly Property Description(item As IItem) As String
    Friend Overridable Function CanEquip(equipSlot As String) As Boolean
        Return False
    End Function
    ReadOnly Property Offer As Integer
    ReadOnly Property Price As Integer
    ReadOnly Property UninstallFee As Integer
    ReadOnly Property InstallFee As Integer
    Overridable ReadOnly Property TechLevel As Integer?
        Get
            Return Nothing
        End Get
    End Property
    MustOverride Function Dialogs(actor As IActor, item As IItem, finalDialog As IDialog) As IReadOnlyDictionary(Of String, IDialog)
    Sub New(
           itemType As String,
           name As String,
           Optional offer As Integer = 0,
           Optional price As Integer = 0,
           Optional installFee As Integer = 0,
           Optional uninstallFee As Integer = 0)
        Me.ItemType = itemType
        Me.Name = name
        Me.Offer = offer
        Me.Price = price
        Me.InstallFee = installFee
        Me.UninstallFee = uninstallFee
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
    Friend Overridable Function GetEntityName(item As IItem) As String
        Return Name
    End Function
End Class

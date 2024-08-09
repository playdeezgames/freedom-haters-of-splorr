Imports FHOS.Data
Imports FHOS.Persistence

Friend Class FuelSupplyItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Private Const FuelCapacityPerMarkValue As Integer = 250
    Private Const PricePerMarkValue As Integer = 500
    Private Const InstallFeePerMarkValue As Integer = 10
    Private Const UninstallFeePerMarkValue As Integer = 5
    Private ReadOnly fuelCapacity As Integer
    Private ReadOnly markType As String

    Public Overrides ReadOnly Property Description(item As IItem) As String
        Get
            Return $"This is the StarLume Fuel {Marks.Descriptors(markType).Name} from Celestial Energy Solutions. Embark on interstellar journeys with StarLume Fuel by Celestial Energy Solutions, the foremost name in propulsion innovation. Crafted from rare celestial minerals and refined through cutting-edge fusion technology, StarLume Fuel guarantees unmatched efficiency and reliability for your spacecraft. Whether you're charting new frontiers or navigating through asteroid belts, trust Celestial Energy Solutions to propel you farther and faster than ever before. Reach for the stars with StarLume Fuel—where limitless possibilities await beyond every horizon."
        End Get
    End Property

    Public Sub New(markType As String)
        MyBase.New(
            ItemTypes.MarkedType(ItemTypes.FuelSupply, markType),
            $"StarLume Fuel {Marks.Descriptors(markType).Name}",
            price:=CalculatePrice(markType),
            installFee:=Marks.Descriptors(markType).Value * InstallFeePerMarkValue,
            uninstallFee:=Marks.Descriptors(markType).Value * UninstallFeePerMarkValue)
        Me.markType = markType
        Me.fuelCapacity = Marks.Descriptors(markType).Value * FuelCapacityPerMarkValue
    End Sub
    Friend Overrides Function CanEquip(equipSlot As String) As Boolean
        Return equipSlot = EquipSlots.PrimaryFuelSupply
    End Function
    Friend Overrides Function Unequip(actor As IActor, item As IItem) As Boolean
        item.Statistics(StatisticTypes.CurrentFuelCapacity) = actor.FuelTank.CurrentValue
        Dim store = actor.FuelTank.MaximumValue = 0
        Return True
    End Function

    Private Shared Function CalculatePrice(markType As String) As Integer
        Return Marks.Descriptors(markType).Value * PricePerMarkValue
    End Function
    Friend Overrides Function Equip(actor As IActor, item As IItem) As Boolean
        Dim store = actor.FuelTank
        store.MaximumValue = item.Statistics(StatisticTypes.FuelCapacity)
        store.CurrentValue = If(item.Statistics(StatisticTypes.CurrentFuelCapacity), 0)
        item.Statistics(StatisticTypes.CurrentFuelCapacity) = Nothing
        Return True
    End Function

    Protected Overrides Sub Initialize(item As IItem)
        item.Statistics(StatisticTypes.FuelCapacity) = fuelCapacity
        item.Statistics(StatisticTypes.CurrentFuelCapacity) = fuelCapacity
    End Sub

    Public Overrides Function Dialogs(actor As IActor, item As IItem, finalDialog As IDialog) As IReadOnlyDictionary(Of String, IDialog)
        Return New Dictionary(Of String, IDialog)
    End Function
End Class

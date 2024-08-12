Imports FHOS.Data
Imports FHOS.Persistence

Friend Class LifeSupportItemTypeDescriptor
    Inherits TechLevelItemTypeDescriptor

    Private Const OxygenCapacityPerMarkValue As Integer = 250
    Private Const PricePerMarkValue As Integer = 500
    Private Const InstallFeePerMarkValue As Integer = 10
    Private Const UninstallFeePerMarkValue As Integer = 5
    Private ReadOnly oxygenCapacity As Integer
    Private ReadOnly markType As String

    Public Overrides ReadOnly Property Description(item As IItem) As IEnumerable(Of String)
        Get
            Dim result As New List(Of String) From {
                $"This is the EterniVita {Marks.Descriptors(markType).Name} from NexGen Dynamics.", "Step into the future with EterniVita, the pinnacle of life support technology.",
                "Engineered to ensure uninterrupted vitality and resilience, EterniVita redefines safety and peace of mind in the most challenging environments.",
                "With its cutting-edge biostasis chambers and adaptive AI monitoring, EterniVita stands as the ultimate safeguard for explorers, colonists, and spacefarers alike.",
                "Embrace limitless possibilities with EterniVita—where every breath guarantees a secure tomorrow, today.",
                $"Tech Level: {item.Statistics(StatisticTypes.TechLevel)}"}
            Dim currentCapacity = item.Statistics(StatisticTypes.CurrentOxygenCapacity)
            Dim maximumCapacity = item.Statistics(StatisticTypes.OxygenCapacity).Value
            If currentCapacity.HasValue Then
                result.Add($"Current Oxygen: {currentCapacity.Value}/{maximumCapacity}")
            Else
                result.Add($"Maximum Oxygen: {maximumCapacity}")
            End If
            Return result
        End Get
    End Property

    Public Sub New(markType As String)
        MyBase.New(
            ItemTypes.MarkedType(ItemTypes.LifeSupport, markType),
            $"EterniVita {Marks.Descriptors(markType).Name}",
            New Dictionary(Of String, Double),
            MarkTypeToTechLevel(markType),
            price:=CalculatePrice(markType),
            installFee:=Marks.Descriptors(markType).Value * InstallFeePerMarkValue,
            uninstallFee:=Marks.Descriptors(markType).Value * UninstallFeePerMarkValue)
        Me.markType = markType
        Me.oxygenCapacity = Marks.Descriptors(markType).Value * OxygenCapacityPerMarkValue
    End Sub

    Private Shared ReadOnly MarkTypeToTechLevel As IReadOnlyDictionary(Of String, Integer) =
        New Dictionary(Of String, Integer) From
        {
            {Marks.MarkI, 1},
            {Marks.MarkII, 3},
            {Marks.MarkIII, 5},
            {Marks.MarkIV, 7},
            {Marks.MarkV, 9}
        }

    Friend Overrides Function CanEquip(equipSlot As String) As Boolean
        Return EquipSlots.Descriptors(equipSlot).Category = EquipSlotCategories.LifeSupportCategory
    End Function
    Friend Overrides Function Unequip(actor As IActor, item As IItem) As Boolean
        Dim store = actor.LifeSupport
        item.Statistics(StatisticTypes.CurrentOxygenCapacity) = store.CurrentValue
        store.MaximumValue = 0
        Return True
    End Function

    Private Shared Function CalculatePrice(markType As String) As Integer
        Return Marks.Descriptors(markType).Value * PricePerMarkValue
    End Function
    Friend Overrides Function Equip(actor As IActor, item As IItem) As Boolean
        Dim store = actor.LifeSupport
        store.MaximumValue = item.Statistics(StatisticTypes.OxygenCapacity)
        store.CurrentValue = If(item.Statistics(StatisticTypes.CurrentOxygenCapacity), 0)
        item.Statistics(StatisticTypes.CurrentOxygenCapacity) = Nothing
        Return True
    End Function

    Protected Overrides Sub Initialize(item As Persistence.IItem)
        item.Statistics(StatisticTypes.OxygenCapacity) = oxygenCapacity
        item.Statistics(StatisticTypes.CurrentOxygenCapacity) = oxygenCapacity
        item.Statistics(StatisticTypes.TechLevel) = TechLevel
    End Sub

    Public Overrides Function Dialogs(actor As IActor, item As IItem, finalDialog As IDialog) As IReadOnlyDictionary(Of String, IDialog)
        Return New Dictionary(Of String, IDialog)
    End Function
End Class

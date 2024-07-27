Friend Class LifeSupportItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Private Const OxygenCapacityPerMarkValue As Integer = 250
    Private Const PricePerMarkValue As Integer = 500
    ReadOnly Property oxygenCapacity As Integer

    Public Sub New(markType As String)
        MyBase.New(
            ItemTypes.MarkedType(ItemTypes.LifeSupport, markType),
            $"EterniVita {Marks.Descriptors(markType).Name}",
            $"This is the EterniVita {Marks.Descriptors(markType).Name} from NexGen Dynamics. Step into the future with EterniVita, the pinnacle of life support technology. Engineered to ensure uninterrupted vitality and resilience, EterniVita redefines safety and peace of mind in the most challenging environments. With its cutting-edge biostasis chambers and adaptive AI monitoring, EterniVita stands as the ultimate safeguard for explorers, colonists, and spacefarers alike. Embrace limitless possibilities with EterniVita—where every breath guarantees a secure tomorrow, today.",
            onEquip:=AddressOf EquipLifeSupportItem,
            onUnequip:=AddressOf UnequipLifeSupportItem,
            price:=CalculatePrice(markType),
            equipSlot:=EquipSlots.LifeSupport)
        Me.oxygenCapacity = Marks.Descriptors(markType).Value * OxygenCapacityPerMarkValue
    End Sub

    Private Shared Sub UnequipLifeSupportItem(actor As Persistence.IActor, item As Persistence.IItem)
        Dim store = actor.Yokes.Store(YokeTypes.LifeSupport)
        item.Statistics(StatisticTypes.CurrentOxygenCapacity) = store.CurrentValue
        store.MaximumValue = 0
    End Sub

    Private Shared Function CalculatePrice(markType As String) As Integer
        Return Marks.Descriptors(markType).Value * PricePerMarkValue
    End Function

    Private Shared Sub EquipLifeSupportItem(actor As Persistence.IActor, item As Persistence.IItem)
        Dim store = actor.Yokes.Store(YokeTypes.LifeSupport)
        store.MaximumValue = item.Statistics(StatisticTypes.OxygenCapacity)
        store.CurrentValue = If(item.Statistics(StatisticTypes.CurrentOxygenCapacity), 0)
    End Sub

    Protected Overrides Sub Initialize(item As Persistence.IItem)
        item.Statistics(StatisticTypes.OxygenCapacity) = oxygenCapacity
        item.Statistics(StatisticTypes.CurrentOxygenCapacity) = oxygenCapacity
    End Sub
End Class

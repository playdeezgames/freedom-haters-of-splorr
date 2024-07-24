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
            price:=CalculatePrice(markType))
        Me.oxygenCapacity = Marks.Descriptors(markType).Value * OxygenCapacityPerMarkValue
    End Sub

    Private Shared Function CalculatePrice(markType As String) As Integer
        Return Marks.Descriptors(markType).Value * PricePerMarkValue
    End Function

    Private Shared Sub EquipLifeSupportItem(actor As Persistence.IActor, item As Persistence.IItem)
        actor.Yokes.Store(YokeTypes.LifeSupport) = actor.Universe.Factory.CreateStore(item.Statistics(StatisticTypes.OxygenCapacity).Value, minimum:=0, maximum:=item.Statistics(StatisticTypes.OxygenCapacity))
    End Sub

    Protected Overrides Sub Initialize(item As Persistence.IItem)
        item.Statistics(StatisticTypes.OxygenCapacity) = oxygenCapacity
    End Sub
End Class

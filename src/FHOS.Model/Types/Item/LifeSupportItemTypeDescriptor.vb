Friend Class LifeSupportItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New(mark As String)
        MyBase.New(
            $"{ItemTypes.LifeSupport}{mark}",
            $"EterniVita {Marks.Descriptors(mark).Name}",
            $"This is the EterniVita {Marks.Descriptors(mark).Name} from NexGen Dynamics. Step into the future with EterniVita, the pinnacle of life support technology. Engineered to ensure uninterrupted vitality and resilience, EterniVita redefines safety and peace of mind in the most challenging environments. With its cutting-edge biostasis chambers and adaptive AI monitoring, EterniVita stands as the ultimate safeguard for explorers, colonists, and spacefarers alike. Embrace limitless possibilities with EterniVita—where every breath guarantees a secure tomorrow, today.",
            equipSlots:=New Dictionary(Of String, Integer) From
            {
                {Model.EquipSlots.LifeSupport, 1}
            },
            onEquip:=AddressOf EquipLifeSupportItem)
    End Sub

    Private Shared Sub EquipLifeSupportItem(actor As Persistence.IActor, item As Persistence.IItem)
        actor.Yokes.Store(YokeTypes.LifeSupport) = actor.Universe.Factory.CreateStore(item.Statistics(StatisticTypes.OxygenCapacity).Value, minimum:=0, maximum:=item.Statistics(StatisticTypes.OxygenCapacity))
    End Sub

    Protected Overrides Sub Initialize(item As Persistence.IItem)
        item.Statistics(StatisticTypes.OxygenCapacity) = 250
    End Sub
End Class

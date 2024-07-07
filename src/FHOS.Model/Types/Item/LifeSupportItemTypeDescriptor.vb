Friend Class LifeSupportItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New(mark As String)
        MyBase.New(
            $"{ItemTypes.LifeSupport}{mark}",
            $"Life Support System {Marks.Descriptors(mark).Name}",
            $"This is the Life Support System {Marks.Descriptors(mark).Name}. It allows organics to breathe, which is useful when the organic wishes to remain not-dead.",
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

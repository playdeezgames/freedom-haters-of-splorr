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
            })
    End Sub

    Protected Overrides Sub Initialize(item As Persistence.IItem)
    End Sub
End Class

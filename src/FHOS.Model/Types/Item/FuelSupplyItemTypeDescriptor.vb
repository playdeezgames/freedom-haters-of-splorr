Imports FHOS.Persistence

Friend Class FuelSupplyItemTypeDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New(mark As String)
        MyBase.New(
            $"{ItemTypes.FuelSupply}{mark}",
            $"Fuel Supply {Marks.Descriptors(mark).Name}",
            $"This is the Fuel Supply {Marks.Descriptors(mark).Name}. You put fuel in it to make yer ship go.",
            equipSlots:=New Dictionary(Of String, Integer) From
            {
                {Model.EquipSlots.FuelSupply, 1}
            })
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
    End Sub
End Class

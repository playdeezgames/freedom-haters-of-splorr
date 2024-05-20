Imports FHOS.Persistence

Friend Class FuelScoopDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(ItemTypes.FuelScoop)
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
        item.Properties.CanRefuel = True
    End Sub
End Class

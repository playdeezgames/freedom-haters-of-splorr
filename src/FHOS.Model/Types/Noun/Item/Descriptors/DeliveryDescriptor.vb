Imports FHOS.Persistence

Friend Class DeliveryDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(ItemTypes.Delivery, "Delivery", "A thing to be delivered.")
    End Sub

    Protected Overrides Sub Initialize(item As IItem)
    End Sub
End Class

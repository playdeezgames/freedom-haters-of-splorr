Imports FHOS.Persistence

Friend Class DeliveryDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New()
        MyBase.New(
            ItemTypes.Delivery,
            "Delivery",
            "A thing to be delivered.",
            toEntityName:=AddressOf DeliveryDescriptorEntityName)
    End Sub

    Private Shared Function DeliveryDescriptorEntityName(item As IItem) As String
        Return item.Metadatas(MetadataTypes.EntityName)
    End Function

    Protected Overrides Sub Initialize(item As IItem)
    End Sub
End Class

Imports FHOS.Persistence

Friend Class DeliveryDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property Description(item As IItem) As String
        Get
            Return "A thing to be delivered."
        End Get
    End Property

    Public Sub New()
        MyBase.New(
            ItemTypes.Delivery,
            "Delivery",
            toEntityName:=AddressOf DeliveryDescriptorEntityName)
    End Sub

    Private Shared Function DeliveryDescriptorEntityName(item As IItem) As String
        Dim planet = item.GetDestinationPlanet
        Return $"{item.Metadatas(MetadataTypes.EntityName)} for {item.Metadatas(MetadataTypes.Recipient)} on {planet.EntityName}"
    End Function

    Protected Overrides Sub Initialize(item As IItem)
    End Sub
End Class

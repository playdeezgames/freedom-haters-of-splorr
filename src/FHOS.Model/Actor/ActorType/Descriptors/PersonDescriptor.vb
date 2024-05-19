Imports FHOS.Persistence

Friend Class PersonDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.Person,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.Person, Hues.LightGray), 1}
            },
            initializer:=AddressOf InitializePerson)
    End Sub

    Private Shared Sub InitializePerson(actor As IActor)
        'TODO
    End Sub

    Friend Overrides Function CanSpawn(location As ILocation) As Boolean
        Return location.LocationType = LocationTypes.Void AndAlso location.Actor Is Nothing
    End Function
End Class

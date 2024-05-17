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
            spawnCount:=0,
            canSpawn:=Function(x) x.LocationType = LocationTypes.Air AndAlso x.Actor Is Nothing,
            initializer:=AddressOf InitializePerson)
    End Sub

    Private Shared Sub InitializePerson(actor As IActor)
        'TODO
    End Sub
End Class

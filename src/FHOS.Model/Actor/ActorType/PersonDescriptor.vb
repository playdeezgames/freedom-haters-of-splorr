Imports FHOS.Persistence

Friend Class PersonDescriptor
    Inherits ActorTypeDescriptor

    Public Sub New()
        MyBase.New(
            ActorTypes.Person,
            {
                ChrW(160),
                ChrW(160),
                ChrW(160),
                ChrW(160)
            },
            LightGray,
            New Dictionary(Of String, Integer) From
            {
                {CostumeTypes.MakeCostume(CostumeTypes.Person, Hues.LightGray), 1}
            },
            maximumFuel:=100,
            spawnCount:=25,
            canSpawn:=Function(x) x.LocationType = LocationTypes.Air AndAlso x.Actor Is Nothing,
            initializer:=AddressOf InitializePerson)
    End Sub

    Private Shared Sub InitializePerson(actor As IActor)
        'TODO
    End Sub
End Class

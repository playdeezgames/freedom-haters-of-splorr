Public Class UniverseData
    Inherits EntityData
    Property Maps As New BucketData(Of MapData)
    Property Actors As New BucketData(Of ActorData)
    Property Locations As New BucketData(Of LocationData)
    Property StarSystems As New BucketData(Of StarSystemData)

    Property LegacyTeleporters As New List(Of TeleporterData)
    Property RecycledTeleporters As New HashSet(Of Integer)
End Class

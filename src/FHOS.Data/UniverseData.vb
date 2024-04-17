Public Class UniverseData
    Inherits EntityData
    Property Maps As New BucketData(Of MapData)
    Property Actors As New BucketData(Of ActorData)

    Property LegacyLocations As New List(Of LocationData)
    Property RecycledLocations As New HashSet(Of Integer)

    Property LegacyStarSystems As New List(Of StarSystemData)
    Property RecycledStarSystems As New HashSet(Of Integer)

    Property LegacyTeleporters As New List(Of TeleporterData)
    Property RecycledTeleporters As New HashSet(Of Integer)
End Class

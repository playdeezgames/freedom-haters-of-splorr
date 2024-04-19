Public Class UniverseData
    Inherits EntityData
    Property Maps As New BucketData(Of MapData)
    Property Actors As New BucketData(Of ActorData)
    Property Locations As New BucketData(Of LocationData)
    Property StarSystems As New BucketData(Of StarSystemData)
    Property Teleporters As New BucketData(Of TeleporterData)
    Property Stars As New BucketData(Of StarData)
    Property Planets As New BucketData(Of PlanetData)
End Class

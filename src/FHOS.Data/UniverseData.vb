Imports System.Text.Json.Serialization

Public Class UniverseData
    Inherits EntityData
    <JsonPropertyName("a1")>
    Property Actors As New BucketData(Of ActorData)
    <JsonPropertyName("l1")>
    Property Locations As New BucketData(Of LocationData)
    <JsonPropertyName("m2")>
    Property Maps As New BucketData(Of MapData)
    <JsonPropertyName("p1")>
    Property PlanetVicinities As New BucketData(Of PlanetVicinityData)
    <JsonPropertyName("p2")>
    Property Planets As New BucketData(Of PlanetData)
    <JsonPropertyName("s2")>
    Property StarVicinities As New BucketData(Of StarVicinityData)
    <JsonPropertyName("s3")>
    Property StarSystems As New BucketData(Of StarSystemData)
    <JsonPropertyName("s4")>
    Property Satellites As New BucketData(Of SatelliteData)
    <JsonPropertyName("t1")>
    Property Teleporters As New BucketData(Of TeleporterData)
End Class

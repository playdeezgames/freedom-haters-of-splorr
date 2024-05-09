Imports System.Text.Json.Serialization

Public Class UniverseData
    Inherits EntityData
    <JsonPropertyName("a1")>
    Property Actors As New BucketData(Of ActorData)
    <JsonPropertyName("l1")>
    Property Locations As New BucketData(Of LocationData)
    <JsonPropertyName("m2")>
    Property Maps As New BucketData(Of MapData)
    <JsonPropertyName("s5")>
    Property Places As New BucketData(Of PlaceData)
    Property Factions As New BucketData(Of FactionData)
    <JsonIgnore>
    Property Messages As New Queue(Of MessageData)
End Class

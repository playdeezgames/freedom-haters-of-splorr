Imports System.Text.Json.Serialization

Public Class UniverseData
    Inherits EntityData
    Property Actors As New BucketData(Of ActorData)
    Property Locations As New BucketData(Of LocationData)
    Property Maps As New BucketData(Of MapData)
    Property Places As New BucketData(Of PlaceData)
    Property Factions As New BucketData(Of FactionData)
    <JsonIgnore>
    Property Messages As New Queue(Of MessageData)
End Class

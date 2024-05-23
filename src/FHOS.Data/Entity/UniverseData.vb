Imports System.Text.Json.Serialization

Public Class UniverseData
    Inherits EntityData
    Property Actors As New BucketData(Of ActorData)
    Property Locations As New BucketData(Of LocationData)
    Property Maps As New BucketData(Of MapData)
    Property Groups As New BucketData(Of GroupData)
    Property Stores As New BucketData(Of StoreData)
    Property Items As New BucketData(Of ItemData)
    Property Avatars As New Stack(Of Integer)
    <JsonIgnore>
    Property Messages As New Queue(Of MessageData)
End Class

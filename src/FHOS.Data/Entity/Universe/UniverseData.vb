Imports System.Text.Json.Serialization

Public Class UniverseData
    Inherits EntityData
    Implements IUniverseData
    Property Actors As New List(Of ActorData) Implements IUniverseData.Actors
    Property Locations As New List(Of LocationData) Implements IUniverseData.Locations
    Property Maps As New BucketData(Of MapData) Implements IUniverseData.Maps
    Property Groups As New BucketData(Of GroupData) Implements IUniverseData.Groups
    Property Stores As New BucketData(Of StoreData) Implements IUniverseData.Stores
    Property Items As New BucketData(Of ItemData) Implements IUniverseData.Items
    Property Avatars As New Stack(Of Integer) Implements IUniverseData.Avatars
    <JsonIgnore>
    Property Messages As New Queue(Of MessageData) Implements IUniverseData.Messages
End Class

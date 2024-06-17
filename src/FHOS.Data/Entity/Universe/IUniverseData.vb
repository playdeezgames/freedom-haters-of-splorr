Imports System.Text.Json.Serialization

Public Interface IUniverseData
    Inherits IEntityData
    Property Actors As List(Of ActorData)
    Property Locations As BucketData(Of LocationData)
    Property Maps As BucketData(Of MapData)
    Property Groups As BucketData(Of GroupData)
    Property Stores As BucketData(Of StoreData)
    Property Items As BucketData(Of ItemData)
    Property Avatars As Stack(Of Integer)
    <JsonIgnore>
    Property Messages As Queue(Of MessageData)
End Interface

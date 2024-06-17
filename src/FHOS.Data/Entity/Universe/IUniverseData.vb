Imports System.Text.Json.Serialization

Public Interface IUniverseData
    Inherits IEntityData
    Property Actors As List(Of ActorData)
    Property Locations As List(Of LocationData)
    Property Maps As List(Of MapData)
    Property Groups As List(Of GroupData)
    Property Stores As List(Of StoreData)
    Property Items As List(Of ItemData)
    Property Avatars As Stack(Of Integer)
    <JsonIgnore>
    Property Messages As Queue(Of MessageData)
End Interface

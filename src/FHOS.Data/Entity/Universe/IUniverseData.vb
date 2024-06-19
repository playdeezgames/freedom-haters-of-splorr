Imports System.Text.Json.Serialization

Public Interface IUniverseData
    Inherits IEntityData
    Property Actors As Dictionary(Of Integer, IActorData)
    Property Locations As Dictionary(Of Integer, ILocationData)
    Property Maps As List(Of IMapData)
    Property Groups As List(Of IGroupData)
    Property Stores As List(Of IStoreData)
    Property Items As List(Of IItemData)
    Property Avatars As Stack(Of Integer)
    <JsonIgnore>
    Property Messages As Queue(Of MessageData)
End Interface

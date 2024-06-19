Imports System.Text.Json.Serialization

Public Interface IUniverseData
    Inherits IEntityData
    Property Actors As Dictionary(Of Integer, IActorData)
    ReadOnly Property NextActorId As Integer
    Property Locations As Dictionary(Of Integer, ILocationData)
    ReadOnly Property NextLocationId As Integer
    Property Maps As List(Of IMapData)
    ReadOnly Property NextMapId As Integer
    Property Groups As List(Of IGroupData)
    ReadOnly Property NextGroupId As Integer
    Property Stores As List(Of IStoreData)
    ReadOnly Property NextStoreId As Integer
    Property Items As List(Of IItemData)
    ReadOnly Property NextItemId As Integer
    Property Avatars As Stack(Of Integer)
    <JsonIgnore>
    Property Messages As Queue(Of MessageData)
End Interface

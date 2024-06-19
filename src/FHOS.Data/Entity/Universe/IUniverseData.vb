Imports System.Text.Json.Serialization

Public Interface IUniverseData
    Inherits IEntityData
    Property Actors As Dictionary(Of Integer, IActorData)
    ReadOnly Property NextActorId As Integer
    Property Locations As Dictionary(Of Integer, ILocationData)
    ReadOnly Property NextLocationId As Integer
    Property Maps As Dictionary(Of Integer, IMapData)
    ReadOnly Property NextMapId As Integer
    Property Groups As Dictionary(Of Integer, IGroupData)
    ReadOnly Property NextGroupId As Integer
    Property Stores As Dictionary(Of Integer, IStoreData)
    ReadOnly Property NextStoreId As Integer
    Property Items As Dictionary(Of Integer, IItemData)
    ReadOnly Property NextItemId As Integer
    Property Avatars As Stack(Of Integer)
    <JsonIgnore>
    Property Messages As Queue(Of MessageData)
End Interface

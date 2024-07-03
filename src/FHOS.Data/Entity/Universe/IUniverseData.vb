Imports System.Text.Json.Serialization

Public Interface IUniverseData
    Inherits IEntityData
    Property Actors As Dictionary(Of Integer, ActorData)
    ReadOnly Property NextActorId As Integer
    Function GetActorData(actorId As Integer) As ActorData
    Property Locations As Dictionary(Of Integer, ILocationData)
    ReadOnly Property NextLocationId As Integer
    Function GetLocationData(locationId As Integer) As ILocationData
    Property Maps As Dictionary(Of Integer, IMapData)
    ReadOnly Property NextMapId As Integer
    Function GetMapData(mapId As Integer) As IMapData
    Property Groups As Dictionary(Of Integer, GroupData)
    ReadOnly Property NextGroupId As Integer
    Function GetGroupData(groupId As Integer) As GroupData
    Property Stores As Dictionary(Of Integer, IStoreData)
    ReadOnly Property NextStoreId As Integer
    Function GetStoreData(storeId As Integer) As IStoreData
    Property Items As Dictionary(Of Integer, ItemData)
    ReadOnly Property NextItemId As Integer
    Function GetItemData(itemId As Integer) As ItemData
    Property Avatars As Stack(Of Integer)
    <JsonIgnore>
    Property Messages As Queue(Of MessageData)
End Interface

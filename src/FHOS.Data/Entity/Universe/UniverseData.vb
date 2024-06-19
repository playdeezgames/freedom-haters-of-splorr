Imports System.Text.Json.Serialization

Public Class UniverseData
    Inherits EntityData
    Implements IUniverseData
    Property Actors As New List(Of IActorData) Implements IUniverseData.Actors
    Property Locations As New List(Of ILocationData) Implements IUniverseData.Locations
    Property Maps As New List(Of IMapData) Implements IUniverseData.Maps
    Property Groups As New List(Of IGroupData) Implements IUniverseData.Groups
    Property Stores As New List(Of IStoreData) Implements IUniverseData.Stores
    Property Items As New List(Of IItemData) Implements IUniverseData.Items
    Property Avatars As New Stack(Of Integer) Implements IUniverseData.Avatars
    <JsonIgnore>
    Property Messages As New Queue(Of MessageData) Implements IUniverseData.Messages
End Class

Imports System.Text.Json.Serialization

Public Class UniverseData
    Inherits EntityData
    Implements IUniverseData
    Public Property Actors As New Dictionary(Of Integer, IActorData) Implements IUniverseData.Actors
    Property Locations As New Dictionary(Of Integer, ILocationData) Implements IUniverseData.Locations
    Property Maps As New Dictionary(Of Integer, IMapData) Implements IUniverseData.Maps
    Property Groups As New Dictionary(Of Integer, IGroupData) Implements IUniverseData.Groups
    Property Stores As New Dictionary(Of Integer, IStoreData) Implements IUniverseData.Stores
    Property Items As New Dictionary(Of Integer, IItemData) Implements IUniverseData.Items
    Property Avatars As New Stack(Of Integer) Implements IUniverseData.Avatars
    <JsonIgnore>
    Property Messages As New Queue(Of MessageData) Implements IUniverseData.Messages
    Private _nextActorId As Integer = 0
    Private _nextLocationId As Integer = 0
    Private _nextMapId As Integer = 0
    Private _nextGroupId As Integer = 0
    Private _nextStoreId As Integer = 0
    Private _nextItemId As Integer = 0
    Public ReadOnly Property NextActorId As Integer Implements IUniverseData.NextActorId
        Get
            Dim actorId = _nextActorId
            _nextActorId += 1
            Return actorId
        End Get
    End Property

    Public ReadOnly Property NextLocationId As Integer Implements IUniverseData.NextLocationId
        Get
            Dim locationId = _nextLocationId
            _nextLocationId += 1
            Return locationId
        End Get
    End Property

    Public ReadOnly Property NextMapId As Integer Implements IUniverseData.NextMapId
        Get
            Dim mapId = _nextMapId
            _nextMapId += 1
            Return mapId
        End Get
    End Property

    Public ReadOnly Property NextGroupId As Integer Implements IUniverseData.NextGroupId
        Get
            Dim groupId = _nextGroupId
            _nextGroupId += 1
            Return groupId
        End Get
    End Property

    Public ReadOnly Property NextStoreId As Integer Implements IUniverseData.NextStoreId
        Get
            Dim storeId = _nextStoreId
            _nextStoreId += 1
            Return storeId
        End Get
    End Property

    Public ReadOnly Property NextItemId As Integer Implements IUniverseData.NextItemId
        Get
            Dim itemId = _nextItemId
            _nextItemId += 1
            Return itemId
        End Get
    End Property
End Class

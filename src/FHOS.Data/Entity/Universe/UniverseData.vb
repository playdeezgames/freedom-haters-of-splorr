Imports System.Text.Json.Serialization

Public Class UniverseData
    Inherits EntityData
    Public Property Actors As New Dictionary(Of Integer, ActorData)
    Property Locations As New Dictionary(Of Integer, LocationData)
    Property Maps As New Dictionary(Of Integer, MapData)
    Property Groups As New Dictionary(Of Integer, GroupData)
    Property Stores As New Dictionary(Of Integer, StoreData)
    Property Items As New Dictionary(Of Integer, ItemData)
    Property Avatars As New Stack(Of Integer)
    <JsonIgnore>
    Property Messages As New Queue(Of MessageData)
    Private _nextActorId As Integer = 0
    Private _nextLocationId As Integer = 0
    Private _nextMapId As Integer = 0
    Private _nextGroupId As Integer = 0
    Private _nextStoreId As Integer = 0
    Private _nextItemId As Integer = 0
    Public ReadOnly Property NextActorId As Integer
        Get
            Dim actorId = _nextActorId
            _nextActorId += 1
            Return actorId
        End Get
    End Property

    Public ReadOnly Property NextLocationId As Integer
        Get
            Dim locationId = _nextLocationId
            _nextLocationId += 1
            Return locationId
        End Get
    End Property

    Public ReadOnly Property NextMapId As Integer
        Get
            Dim mapId = _nextMapId
            _nextMapId += 1
            Return mapId
        End Get
    End Property

    Public ReadOnly Property NextGroupId As Integer
        Get
            Dim groupId = _nextGroupId
            _nextGroupId += 1
            Return groupId
        End Get
    End Property

    Public ReadOnly Property NextStoreId As Integer
        Get
            Dim storeId = _nextStoreId
            _nextStoreId += 1
            Return storeId
        End Get
    End Property

    Public ReadOnly Property NextItemId As Integer
        Get
            Dim itemId = _nextItemId
            _nextItemId += 1
            Return itemId
        End Get
    End Property

    Public Function GetActorData(actorId As Integer) As ActorData
        Dim actorData As ActorData = Nothing
        If Actors.TryGetValue(actorId, actorData) Then
            Return actorData
        End If
        Return Nothing
    End Function

    Public Function GetLocationData(locationId As Integer) As LocationData
        Return Nothing
    End Function

    Public Function GetMapData(mapId As Integer) As MapData
        Return Nothing
    End Function

    Public Function GetGroupData(groupId As Integer) As GroupData
        Return Nothing
    End Function

    Public Function GetStoreData(storeId As Integer) As StoreData
        Return Nothing
    End Function

    Public Function GetItemData(storeId As Integer) As ItemData
        Return Nothing
    End Function
End Class

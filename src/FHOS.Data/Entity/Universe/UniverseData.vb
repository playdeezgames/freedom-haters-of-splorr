Imports System.Text.Json.Serialization

Public Class UniverseData
    Inherits EntityData
    Public Property Actors As New Dictionary(Of Integer, ActorData)
    Property Locations As New Dictionary(Of Integer, LocationData)
    Property Maps As New Dictionary(Of Integer, MapData)
    Property Groups As New Dictionary(Of Integer, GroupData)
    Property Stores As New Dictionary(Of Integer, StoreData)
    Property Items As New Dictionary(Of Integer, ItemData)
    Property Avatar As Integer?
    <JsonIgnore>
    Property Messages As New Queue(Of MessageData)
    Private _nextActorId As Integer? = Nothing
    Private _nextLocationId As Integer? = Nothing
    Private _nextMapId As Integer? = Nothing
    Private _nextGroupId As Integer? = Nothing
    Private _nextStoreId As Integer? = Nothing
    Private _nextItemId As Integer? = Nothing
    Public ReadOnly Property NextActorId As Integer
        Get
            If Not _nextActorId.HasValue Then
                _nextActorId = If(Not Actors.Any, 0, Actors.Keys.Max + 1)
            End If
            Dim actorId = _nextActorId.Value
            _nextActorId = actorId + 1
            Return actorId
        End Get
    End Property

    Public ReadOnly Property NextLocationId As Integer
        Get
            If Not _nextLocationId.HasValue Then
                _nextLocationId = If(Not Locations.Any, 0, Locations.Keys.Max + 1)
            End If
            Dim locationId = _nextLocationId.Value
            _nextLocationId = locationId + 1
            Return locationId
        End Get
    End Property

    Public ReadOnly Property NextMapId As Integer
        Get
            If Not _nextMapId.HasValue Then
                _nextMapId = If(Not Maps.Any, 0, Maps.Keys.Max + 1)
            End If
            Dim mapId = _nextMapId.Value
            _nextMapId = mapId + 1
            Return mapId
        End Get
    End Property

    Public ReadOnly Property NextGroupId As Integer
        Get
            If Not _nextGroupId.HasValue Then
                _nextGroupId = If(Not Groups.Any, 0, Groups.Keys.Max + 1)
            End If
            Dim groupId = _nextGroupId.Value
            _nextGroupId = groupId + 1
            Return groupId
        End Get
    End Property

    Public ReadOnly Property NextStoreId As Integer
        Get
            If Not _nextStoreId.HasValue Then
                _nextStoreId = If(Not Stores.Any, 0, Stores.Keys.Max + 1)
            End If
            Dim storeId = _nextStoreId.Value
            _nextStoreId = storeId + 1
            Return storeId
        End Get
    End Property

    Public ReadOnly Property NextItemId As Integer
        Get
            If Not _nextItemId.HasValue Then
                _nextItemId = If(Not Items.Any, 0, Items.Keys.Max + 1)
            End If
            Dim itemId = _nextItemId.Value
            _nextItemId = itemId + 1
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
End Class

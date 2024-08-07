﻿Imports FHOS.Data

Friend Class Map
    Inherits MapDataClient
    Implements IMap

    Protected Sub New(universeData As UniverseData, mapId As Integer)
        MyBase.New(universeData, mapId)
    End Sub

    Friend Shared Function FromId(universeData As UniverseData, mapId As Integer?) As IMap
        If mapId.HasValue Then
            Return New Map(universeData, mapId.Value)
        End If
        Return Nothing
    End Function

    Public ReadOnly Property Locations As IEnumerable(Of ILocation) Implements IMap.Locations
        Get
            Return EntityData.AllLocations.Select(Function(x) Location.FromId(UniverseData, x))
        End Get
    End Property

    Public ReadOnly Property Size As (Columns As Integer, Rows As Integer) Implements IMap.Size
        Get
            Return (EntityData.GetStatistic(PersistenceStatisticTypes.Columns).Value, EntityData.GetStatistic(PersistenceStatisticTypes.Rows).Value)
        End Get
    End Property

    Public Property YokedGroup(yokeType As String) As IGroup Implements IMap.YokedGroup
        Get
            Return Group.FromId(UniverseData, EntityData.GetYokedGroup(yokeType))
        End Get
        Set(value As IGroup)
            EntityData.SetYokedGroup(yokeType, value?.Id)
        End Set
    End Property

    Public Function GetLocation(column As Integer, row As Integer) As ILocation Implements IMap.GetLocation
        If column < 0 OrElse row < 0 OrElse column >= EntityData.GetStatistic(PersistenceStatisticTypes.Columns).Value OrElse row >= EntityData.GetStatistic(PersistenceStatisticTypes.Rows).Value Then
            Return Nothing
        End If
        Dim index = column + row * EntityData.GetStatistic(PersistenceStatisticTypes.Columns).Value
        Return Location.FromId(UniverseData, EntityData.GetLocation(index))
    End Function
End Class

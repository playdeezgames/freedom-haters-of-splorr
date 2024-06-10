Imports FHOS.Data

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
            Return EntityData.Locations.Select(Function(x) Location.FromId(UniverseData, x))
        End Get
    End Property

    Public ReadOnly Property Size As (Columns As Integer, Rows As Integer) Implements IMap.Size
        Get
            Return (EntityData.Statistics(PersistenceStatisticTypes.Columns), EntityData.Statistics(PersistenceStatisticTypes.Rows))
        End Get
    End Property

    Public Property YokedGroup(yokeType As String) As IGroup Implements IMap.YokedGroup
        Get
            Dim groupId As Integer
            If EntityData.YokedGroups.TryGetValue(yokeType, groupId) Then
                Return Group.FromId(UniverseData, groupId)
            End If
            Return Nothing
        End Get
        Set(value As IGroup)
            If value IsNot Nothing Then
                EntityData.YokedGroups(yokeType) = value.Id
            Else
                EntityData.YokedGroups.Remove(yokeType)
            End If
        End Set
    End Property

    Public Function GetLocation(column As Integer, row As Integer) As ILocation Implements IMap.GetLocation
        If column < 0 OrElse row < 0 OrElse column >= EntityData.Statistics(PersistenceStatisticTypes.Columns) OrElse row >= EntityData.Statistics(PersistenceStatisticTypes.Rows) Then
            Return Nothing
        End If
        Return Location.FromId(UniverseData, EntityData.Locations(column + row * EntityData.Statistics(PersistenceStatisticTypes.Columns)))
    End Function
End Class

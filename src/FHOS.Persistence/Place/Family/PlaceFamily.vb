Friend Class PlaceFamily
    Inherits PlaceDataClient
    Implements IPlaceFamily

    Public Sub New(universeData As Data.UniverseData, placeId As Integer)
        MyBase.New(universeData, placeId)
    End Sub

    Public ReadOnly Property Parent As IPlace Implements IPlaceFamily.Parent
        Get
            Dim parentId As Integer
            If EntityData.Statistics.TryGetValue(StatisticTypes.ParentId, parentId) Then
                Return Place.FromId(UniverseData, parentId)
            End If
            Return Nothing
        End Get
    End Property

    Public Property PlanetCount As Integer Implements IPlaceFamily.PlanetCount
        Get
            Return If(GetStatistic(StatisticTypes.PlanetCount), 0)
        End Get
        Set(value As Integer)
            EntityData.Statistics(StatisticTypes.PlanetCount) = value
        End Set
    End Property

    Public Property SatelliteCount As Integer Implements IPlaceFamily.SatelliteCount
        Get
            Return If(GetStatistic(StatisticTypes.SatelliteCount), 0)
        End Get
        Set(value As Integer)
            EntityData.Statistics(StatisticTypes.SatelliteCount) = value
        End Set
    End Property

    Friend Shared Function FromId(universeData As Data.UniverseData, id As Integer) As IPlaceFamily
        Return New PlaceFamily(universeData, id)
    End Function
End Class

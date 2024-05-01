Friend Class Place
    Inherits PlaceDataClient
    Implements IPlace

    Public Sub New(universeData As Data.UniverseData, placeId As Integer)
        MyBase.New(universeData, placeId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IPlace.Id
        Get
            Return PlaceId
        End Get
    End Property

    Public ReadOnly Property Universe As IUniverse Implements IPlace.Universe
        Get
            Return New Universe(UniverseData)
        End Get
    End Property


    Public ReadOnly Property Name As String Implements IPlace.Name
        Get
            Return PlaceData.Metadatas(MetadataTypes.Name)
        End Get
    End Property

    Public Property Map As IMap Implements IPlace.Map
        Get
            Dim mapId As Integer
            If PlaceData.Statistics.TryGetValue(StatisticTypes.MapId, mapId) Then
                Return New Map(UniverseData, mapId)
            End If
            Return Nothing
        End Get
        Set(value As IMap)
            If value Is Nothing Then
                PlaceData.Statistics.Remove(StatisticTypes.MapId)
            Else
                PlaceData.Statistics(StatisticTypes.MapId) = value.Id
            End If
        End Set
    End Property

    Public ReadOnly Property Identifier As String Implements IPlace.Identifier
        Get
            Return PlaceData.Metadatas(MetadataTypes.Identifier)
        End Get
    End Property
End Class

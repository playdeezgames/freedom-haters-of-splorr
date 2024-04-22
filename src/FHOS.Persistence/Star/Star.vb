Friend Class Star
    Inherits StarDataClient
    Implements IStar

    Public Sub New(universeData As Data.UniverseData, starId As Integer)
        MyBase.New(universeData, starId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IStar.Id
        Get
            Return StarId
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IStar.Name
        Get
            Return StarData.Metadatas(MetadataTypes.Name)
        End Get
    End Property

    Public ReadOnly Property Universe As IUniverse Implements IStar.Universe
        Get
            Return New Universe(UniverseData)
        End Get
    End Property

    Public Property Map As IMap Implements IStar.Map
        Get
            Dim mapId As Integer
            If StarData.Statistics.TryGetValue(StatisticTypes.MapId, mapId) Then
                Return New Map(UniverseData, mapId)
            End If
            Return Nothing
        End Get
        Set(value As IMap)
            If value Is Nothing Then
                StarData.Statistics.Remove(StatisticTypes.MapId)
            Else
                StarData.Statistics(StatisticTypes.MapId) = value.Id
            End If
        End Set
    End Property
End Class

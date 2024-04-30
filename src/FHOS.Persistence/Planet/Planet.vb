Friend Class Planet
    Inherits PlaceDataClient
    Implements IPlanet

    Public Sub New(universeData As Data.UniverseData, planetId As Integer)
        MyBase.New(universeData, planetId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IPlanet.Id
        Get
            Return PlaceId
        End Get
    End Property

    Public ReadOnly Property PlanetType As String Implements IPlanet.PlanetType
        Get
            Return PlaceData.Metadatas(MetadataTypes.PlanetType)
        End Get
    End Property
End Class

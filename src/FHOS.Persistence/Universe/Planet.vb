Friend Class Planet
    Inherits PlanetDataClient
    Implements IPlanet

    Public Sub New(universeData As Data.UniverseData, planetId As Integer)
        MyBase.New(universeData, planetId)
    End Sub

    Public ReadOnly Property Id As Integer Implements IPlanet.Id
        Get
            Return PlanetId
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IPlanet.Name
        Get
            Return PlanetData.Metadatas(MetadataTypes.Name)
        End Get
    End Property
End Class

Friend Class Planet
    Inherits Place
    Implements IPlanet

    Public Sub New(universeData As Data.UniverseData, planetId As Integer)
        MyBase.New(universeData, planetId)
    End Sub

    Public ReadOnly Property PlanetType As String Implements IPlanet.PlanetType
        Get
            Return PlaceData.Metadatas(MetadataTypes.PlanetType)
        End Get
    End Property
End Class

Imports FHOS.Data

Friend Class PlanetDataClient
    Inherits UniverseDataClient
    Protected ReadOnly PlanetId As Integer
    Protected ReadOnly Property PlanetData As PlanetData
        Get
            Return UniverseData.Planets.Entities(PlanetId)
        End Get
    End Property
    Public Sub New(universeData As Data.UniverseData, planetId As Integer)
        MyBase.New(universeData)
        Me.PlanetId = planetId
    End Sub
End Class

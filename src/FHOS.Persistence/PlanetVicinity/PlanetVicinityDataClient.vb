Imports FHOS.Data

Friend Class PlanetVicinityDataClient
    Inherits UniverseDataClient
    Protected ReadOnly PlanetVicinityId As Integer
    Protected ReadOnly Property PlanetVicinityData As PlanetVicinityData
        Get
            Return UniverseData.PlanetVicinities.Entities(PlanetVicinityId)
        End Get
    End Property
    Public Sub New(universeData As Data.UniverseData, planetId As Integer)
        MyBase.New(universeData)
        Me.PlanetVicinityId = planetId
    End Sub
End Class

Friend Class PlanetDataClient
    Inherits UniverseDataClient
    Protected ReadOnly PlanetId As Integer
    Public Sub New(universeData As Data.UniverseData, planetId As Integer)
        MyBase.New(universeData)
        Me.PlanetId = planetId
    End Sub
End Class

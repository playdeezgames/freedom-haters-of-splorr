Imports FHOS.Data

Friend Class PlanetVicinity
    Inherits Planet
    Implements IPlanetVicinity

    Public Sub New(universeData As Data.UniverseData, planetId As Integer)
        MyBase.New(universeData, planetId)
    End Sub
End Class

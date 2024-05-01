Friend Class Satellite
    Inherits Place
    Implements ISatellite

    Public Sub New(universeData As Data.UniverseData, placeId As Integer)
        MyBase.New(universeData, placeId)
    End Sub
End Class

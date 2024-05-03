Imports FHOS.Data

Friend Class StarVicinity
    Inherits Star
    Implements IStarVicinity

    Public Sub New(universeData As Data.UniverseData, placeId As Integer)
        MyBase.New(universeData, placeId)
    End Sub
End Class

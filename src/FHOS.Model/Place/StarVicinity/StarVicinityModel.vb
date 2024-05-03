Friend Class StarVicinityModel
    Inherits PlaceModel
    Implements IStarVicinityModel

    Private ReadOnly starVicinity As Persistence.IPlace

    Public Sub New(starVicinity As Persistence.IPlace)
        MyBase.New(starVicinity)
        Me.starVicinity = starVicinity
    End Sub
End Class

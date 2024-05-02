Friend Class StarVicinityModel
    Inherits PlaceModel
    Implements IStarVicinityModel

    Private ReadOnly starVicinity As Persistence.IStarVicinity

    Public Sub New(starVicinity As Persistence.IStarVicinity)
        MyBase.New(starVicinity)
        Me.starVicinity = starVicinity
    End Sub
End Class

Friend Class StarVicinityModel
    Implements IStarVicinityModel

    Private ReadOnly starVicinity As Persistence.IStarVicinity

    Public Sub New(starVicinity As Persistence.IStarVicinity)
        Me.starVicinity = starVicinity
    End Sub

    Public ReadOnly Property Name As String Implements IStarVicinityModel.Name
        Get
            Return starVicinity.Name
        End Get
    End Property
End Class

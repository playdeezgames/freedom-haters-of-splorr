Friend Class PlanetVicinityModel
    Implements IPlanetVicinityModel

    Private ReadOnly planet As Persistence.IPlanetVicinity

    Public Sub New(planet As Persistence.IPlanetVicinity)
        Me.planet = planet
    End Sub

    Public ReadOnly Property Name As String Implements IPlanetVicinityModel.Name
        Get
            Return planet.Name
        End Get
    End Property
End Class

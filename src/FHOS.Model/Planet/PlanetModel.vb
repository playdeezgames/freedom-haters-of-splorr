Friend Class PlanetModel
    Implements IPlanetModel

    Private ReadOnly planet As Persistence.IPlanetVicinity

    Public Sub New(planet As Persistence.IPlanetVicinity)
        Me.planet = planet
    End Sub

    Public ReadOnly Property Name As String Implements IPlanetModel.Name
        Get
            Return planet.Name
        End Get
    End Property
End Class

Imports FHOS.Persistence

Friend Class PlanetModel
    Inherits PlaceModel
    Implements IPlanetModel

    Private ReadOnly planet As Persistence.IPlace

    Public Sub New(planet As Persistence.IPlace)
        MyBase.New(planet)
        Me.planet = planet
    End Sub
End Class

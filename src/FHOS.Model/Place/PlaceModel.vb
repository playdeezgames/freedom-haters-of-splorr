Imports FHOS.Persistence

Friend Class PlaceModel
    Implements IPlaceModel
    Private ReadOnly place As IPlace

    Sub New(place As IPlace)
        Me.place = place
    End Sub

    ReadOnly Property Name As String Implements IPlaceModel.Name
        Get
            Return place.Name
        End Get
    End Property

    Public ReadOnly Property CanRefillOxygen As Boolean Implements IPlaceModel.CanRefillOxygen
        Get
            If place.PlanetType Is Nothing OrElse place.PlaceType <> PlaceTypes.Planet Then
                Return False
            End If
            Return PlanetTypes.Descriptors(place.PlanetType).CanRefillOxygen
        End Get
    End Property

    Public ReadOnly Property StarType As String Implements IPlaceModel.StarType
        Get
            Return place.StarType
        End Get
    End Property

    Public ReadOnly Property PlanetVicinityCount As Integer Implements IPlaceModel.PlanetVicinityCount
        Get
            Return place.PlanetVicinityCount
        End Get
    End Property

    Public ReadOnly Property X As Integer Implements IPlaceModel.X
        Get
            Return place.X
        End Get
    End Property

    Public ReadOnly Property Y As Integer Implements IPlaceModel.Y
        Get
            Return place.Y
        End Get
    End Property

    Public ReadOnly Property SatelliteCount As Integer Implements IPlaceModel.SatelliteCount
        Get
            Return place.SatelliteCount
        End Get
    End Property

    Public ReadOnly Property PlanetType As String Implements IPlaceModel.PlanetType
        Get
            Return place.PlanetType
        End Get
    End Property
End Class

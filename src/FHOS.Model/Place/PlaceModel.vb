Imports FHOS.Persistence

Friend Class PlaceModel
    Implements IPlaceModel
    Private ReadOnly place As IPlace

    Protected Sub New(place As IPlace)
        Me.place = place
    End Sub

    Friend Shared Function FromPlace(place As IPlace) As IPlaceModel
        If place Is Nothing Then
            Return Nothing
        End If
        Return New PlaceModel(place)
    End Function

    ReadOnly Property Name As String Implements IPlaceModel.Name
        Get
            Return place.Name
        End Get
    End Property

    Public ReadOnly Property CanRefillOxygen As Boolean Implements IPlaceModel.CanRefillOxygen
        Get
            If place.Subtype Is Nothing OrElse place.PlaceType <> PlaceTypes.Planet Then
                Return False
            End If
            Return PlanetTypes.Descriptors(place.Subtype).CanRefillOxygen
        End Get
    End Property

    Public ReadOnly Property StarType As String Implements IPlaceModel.StarType
        Get
            Return place.Subtype
        End Get
    End Property

    Public ReadOnly Property PlanetVicinityCount As Integer Implements IPlaceModel.PlanetVicinityCount
        Get
            Return place.PlanetVicinityCount
        End Get
    End Property

    Public ReadOnly Property X As Integer Implements IPlaceModel.X
        Get
            Return place.Position.X
        End Get
    End Property

    Public ReadOnly Property Y As Integer Implements IPlaceModel.Y
        Get
            Return place.Position.Y
        End Get
    End Property

    Public ReadOnly Property SatelliteCount As Integer Implements IPlaceModel.SatelliteCount
        Get
            Return place.SatelliteCount
        End Get
    End Property

    Public ReadOnly Property PlanetType As String Implements IPlaceModel.PlanetType
        Get
            Return place.Subtype
        End Get
    End Property

    Public ReadOnly Property Parent As IPlaceModel Implements IPlaceModel.Parent
        Get
            Dim parentPlace = place.Parent
            If parentPlace IsNot Nothing Then
                Return New PlaceModel(parentPlace)
            End If
            Return Nothing
        End Get
    End Property
End Class

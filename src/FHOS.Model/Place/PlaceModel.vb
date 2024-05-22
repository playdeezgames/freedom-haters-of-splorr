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
            Return place.Properties.Name
        End Get
    End Property

    Public ReadOnly Property Subtype As String Implements IPlaceModel.Subtype
        Get
            Return place.Subtype
        End Get
    End Property

    Public ReadOnly Property PlanetCount As Integer Implements IPlaceModel.PlanetCount
        Get
            Return place.Family.PlanetCount
        End Get
    End Property

    Public ReadOnly Property X As Integer Implements IPlaceModel.X
        Get
            Return place.Properties.Position.X
        End Get
    End Property

    Public ReadOnly Property Y As Integer Implements IPlaceModel.Y
        Get
            Return place.Properties.Position.Y
        End Get
    End Property

    Public ReadOnly Property SatelliteCount As Integer Implements IPlaceModel.SatelliteCount
        Get
            Return place.Family.SatelliteCount
        End Get
    End Property

    Public ReadOnly Property Parent As IPlaceModel Implements IPlaceModel.Parent
        Get
            Dim parentPlace = place.Family.Parent
            If parentPlace IsNot Nothing Then
                Return New PlaceModel(parentPlace)
            End If
            Return Nothing
        End Get
    End Property
End Class

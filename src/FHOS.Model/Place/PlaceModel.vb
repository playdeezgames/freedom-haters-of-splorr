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
End Class

Imports FHOS.Persistence

Friend MustInherit Class PlaceModel
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
End Class

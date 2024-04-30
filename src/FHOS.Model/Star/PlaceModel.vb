Imports FHOS.Persistence

Friend Class PlaceModel
    Implements IPlaceModel

    Private ReadOnly place As IPlace

    Public Sub New(place As IPlace)
        Me.place = place
    End Sub
End Class

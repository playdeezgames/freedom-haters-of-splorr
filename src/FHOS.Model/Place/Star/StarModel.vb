Imports FHOS.Persistence

Friend Class StarModel
    Inherits PlaceModel
    Implements IStarModel

    Private ReadOnly star As IPlace

    Public Sub New(star As IPlace)
        MyBase.New(star)
        Me.star = star
    End Sub
End Class

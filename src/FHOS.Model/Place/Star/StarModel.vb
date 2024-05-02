Imports FHOS.Persistence

Friend Class StarModel
    Inherits PlaceModel
    Implements IStarModel

    Private ReadOnly star As IStar

    Public Sub New(star As IStar)
        MyBase.New(star)
        Me.star = star
    End Sub
End Class

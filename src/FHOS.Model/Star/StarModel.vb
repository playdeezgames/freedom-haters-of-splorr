Imports FHOS.Persistence

Friend Class StarModel
    Implements IStarModel

    Private star As IStar

    Public Sub New(star As IStar)
        Me.star = star
    End Sub
End Class

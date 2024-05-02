Imports FHOS.Persistence

Friend Class StarModel
    Implements IStarModel

    Private ReadOnly star As IStar

    Public Sub New(star As IStar)
        Me.star = star
    End Sub

    Public ReadOnly Property Name As String Implements IStarModel.Name
        Get
            Return star.Name
        End Get
    End Property
End Class

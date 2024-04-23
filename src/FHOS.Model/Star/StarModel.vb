Friend Class StarModel
    Implements IStarModel

    Private star As Persistence.IStarVicinity

    Public Sub New(star As Persistence.IStarVicinity)
        Me.star = star
    End Sub

    Public ReadOnly Property Name As String Implements IStarModel.Name
        Get
            Return star.Name
        End Get
    End Property
End Class

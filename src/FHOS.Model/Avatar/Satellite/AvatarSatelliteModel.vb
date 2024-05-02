Imports FHOS.Persistence

Friend Class AvatarSatelliteModel
    Implements IAvatarSatelliteModel

    Private ReadOnly avatar As IActor

    Public Sub New(avatar As IActor)
        Me.avatar = avatar
    End Sub

    Public ReadOnly Property Current As ISatelliteModel Implements IAvatarSatelliteModel.Current
        Get
            If avatar.Location.Satellite IsNot Nothing Then
                Return New SatelliteModel(avatar.Location.Satellite)
            End If
            Return Nothing
        End Get
    End Property
End Class

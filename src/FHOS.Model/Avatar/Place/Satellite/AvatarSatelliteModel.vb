Imports FHOS.Persistence

Friend Class AvatarSatelliteModel
    Inherits AvatarPlaceModel
    Implements IAvatarSatelliteModel

    Public Sub New(avatar As IActor)
        MyBase.New(avatar)
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

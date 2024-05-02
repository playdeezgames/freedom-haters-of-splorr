Imports FHOS.Persistence

Friend Class AvatarPlaceModel
    Inherits BaseAvatarModel
    Implements IAvatarPlaceModel

    Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property Current As IPlaceModel Implements IAvatarPlaceModel.Current
        Get
            If avatar.Location.Place IsNot Nothing Then
                Return New PlaceModel(avatar.Location.Place)
            End If
            Return Nothing
        End Get
    End Property
End Class

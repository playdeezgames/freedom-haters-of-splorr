Imports FHOS.Persistence

Friend Class AvatarStarModel
    Inherits AvatarPlaceModel
    Implements IAvatarStarModel

    Public Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property LegacyCurrent As IStarModel Implements IAvatarStarModel.LegacyCurrent
        Get
            If avatar.Location.Place IsNot Nothing AndAlso avatar.Location.Place.PlaceType = PlaceTypes.Star Then
                Return New StarModel(avatar.Location.Place)
            End If
            Return Nothing
        End Get
    End Property

End Class

Imports FHOS.Persistence

Friend MustInherit Class AvatarPlaceModel
    Inherits BaseAvatarModel
    Implements IAvatarPlaceModel

    Protected Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub
End Class

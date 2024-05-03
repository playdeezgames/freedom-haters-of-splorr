Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarStarSystemModel
    Inherits AvatarPlaceModel
    Implements IAvatarStarSystemModel

    Public Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property LegacyCurrent As IStarSystemModel Implements IAvatarStarSystemModel.LegacyCurrent
        Get
            If avatar.Location.Place Is Nothing Then
                Return Nothing
            End If
            Return New StarSystemModel(avatar.Location.Place)
        End Get
    End Property
End Class

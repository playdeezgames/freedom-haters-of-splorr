Imports FHOS.Persistence
Imports SPLORR.Game

Friend Class AvatarStarVicinityModel
    Inherits AvatarPlaceModel
    Implements IAvatarStarVicinityModel

    Public Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property LegacyCurrent As IStarVicinityModel Implements IAvatarStarVicinityModel.LegacyCurrent
        Get
            Dim star = avatar.Location.Place
            If star IsNot Nothing Then
                Return New StarVicinityModel(star)
            End If
            Return Nothing
        End Get
    End Property
End Class

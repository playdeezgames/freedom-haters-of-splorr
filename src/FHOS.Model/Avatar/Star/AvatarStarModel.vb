Imports FHOS.Persistence

Friend Class AvatarStarModel
    Inherits BaseAvatarModel
    Implements IAvatarStarModel

    Public Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property CanRefillFuel As Boolean Implements IAvatarStarModel.CanRefillFuel
        Get
            Return Current IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property Current As IStarModel Implements IAvatarStarModel.Current
        Get
            If avatar.Location.Star IsNot Nothing Then
                Return New StarModel(avatar.Location.Star)
            End If
            Return Nothing
        End Get
    End Property

    Public Sub Refuel() Implements IAvatarStarModel.Refuel
        If CanRefillFuel Then
            avatar.Fuel = avatar.MaximumFuel
        End If
    End Sub
End Class

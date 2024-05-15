Imports FHOS.Persistence

Friend Class AvatarStatusModel
    Inherits BaseAvatarModel
    Implements IAvatarStatusModel

    Protected Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property IsGameOver As Boolean Implements IAvatarStatusModel.IsGameOver
        Get
            Return IsDead OrElse IsBankrupt
        End Get
    End Property

    Public ReadOnly Property IsDead As Boolean Implements IAvatarStatusModel.IsDead
        Get
            Return avatar.LifeSupport.CurrentValue = avatar.LifeSupport.MinimumValue.Value
        End Get
    End Property

    Public ReadOnly Property IsBankrupt As Boolean Implements IAvatarStatusModel.IsBankrupt
        Get
            Return avatar.Jools < avatar.MinimumJools
        End Get
    End Property

    Friend Shared Function FromActor(avatar As IActor) As IAvatarStatusModel
        Return New AvatarStatusModel(avatar)
    End Function
End Class

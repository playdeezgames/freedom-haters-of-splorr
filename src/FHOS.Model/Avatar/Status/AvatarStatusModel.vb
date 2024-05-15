Imports FHOS.Persistence

Friend Class AvatarStatusModel
    Inherits BaseAvatarModel
    Implements IAvatarStatusModel

    Protected Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property GameOver As Boolean Implements IAvatarStatusModel.GameOver
        Get
            Return Dead OrElse Bankrupt
        End Get
    End Property

    Public ReadOnly Property Dead As Boolean Implements IAvatarStatusModel.Dead
        Get
            Return avatar.LifeSupport.CurrentValue = avatar.LifeSupport.MinimumValue.Value
        End Get
    End Property

    Public ReadOnly Property Bankrupt As Boolean Implements IAvatarStatusModel.Bankrupt
        Get
            Return avatar.Jools < avatar.MinimumJools
        End Get
    End Property

    Friend Shared Function FromActor(avatar As IActor) As IAvatarStatusModel
        Return New AvatarStatusModel(avatar)
    End Function
End Class

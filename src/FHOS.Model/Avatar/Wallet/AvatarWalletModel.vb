Imports FHOS.Persistence

Friend Class AvatarWalletModel
    Inherits BaseAvatarModel
    Implements IAvatarWalletModel

    Protected Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property Jools As Integer Implements IAvatarWalletModel.Jools
        Get
            Return avatar.Jools
        End Get
    End Property

    Public ReadOnly Property MinimumJools As Integer Implements IAvatarWalletModel.MinimumJools
        Get
            Return avatar.MinimumJools
        End Get
    End Property

    Friend Shared Function FromActor(avatar As IActor) As IAvatarWalletModel
        Return New AvatarWalletModel(avatar)
    End Function
End Class

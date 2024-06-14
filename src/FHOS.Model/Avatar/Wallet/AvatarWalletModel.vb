Imports FHOS.Persistence

Friend Class AvatarWalletModel
    Inherits BaseAvatarModel
    Implements IAvatarWalletModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Public ReadOnly Property Jools As Integer Implements IAvatarWalletModel.Jools
        Get
            Return actor.YokedStore(YokeTypes.Wallet).CurrentValue
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarWalletModel
        Return New AvatarWalletModel(actor)
    End Function
End Class

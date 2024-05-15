Imports FHOS.Persistence

Friend Class AvatarInteractionModel
    Inherits BaseAvatarModel
    Implements IAvatarInteractionModel

    Protected Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property IsActive As Boolean Implements IAvatarInteractionModel.IsActive
        Get
            Return avatar.Interactor IsNot Nothing
        End Get
    End Property

    Public Sub Leave() Implements IAvatarInteractionModel.Leave
        avatar.Interactor = Nothing
    End Sub

    Friend Shared Function FromActor(avatar As IActor) As IAvatarInteractionModel
        Return New AvatarInteractionModel(avatar)
    End Function
End Class

Imports FHOS.Persistence

Friend Class AvatarInteractionModel
    Inherits BaseAvatarModel
    Implements IAvatarInteractionModel

    Protected Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public ReadOnly Property IsInteracting As Boolean Implements IAvatarInteractionModel.IsInteracting
        Get
            Return avatar.Interactor IsNot Nothing
        End Get
    End Property

    Public Sub LeaveInteraction() Implements IAvatarInteractionModel.LeaveInteraction
        avatar.Interactor = Nothing
    End Sub

    Friend Shared Function FromActor(avatar As IActor) As IAvatarInteractionModel
        Return New AvatarInteractionModel(avatar)
    End Function
End Class

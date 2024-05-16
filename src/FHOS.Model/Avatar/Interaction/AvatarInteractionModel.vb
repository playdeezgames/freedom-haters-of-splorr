Imports FHOS.Persistence

Friend Class AvatarInteractionModel
    Inherits BaseAvatarModel
    Implements IAvatarInteractionModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Public ReadOnly Property IsActive As Boolean Implements IAvatarInteractionModel.IsActive
        Get
            Return actor.Interactor IsNot Nothing
        End Get
    End Property

    Public Sub Leave() Implements IAvatarInteractionModel.Leave
        actor.Interactor = Nothing
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarInteractionModel
        Return New AvatarInteractionModel(actor)
    End Function
End Class

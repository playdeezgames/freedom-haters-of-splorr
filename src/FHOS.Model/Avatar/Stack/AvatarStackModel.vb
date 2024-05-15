Imports FHOS.Persistence

Friend Class AvatarStackModel
    Inherits BaseAvatarModel
    Implements IAvatarStackModel

    Protected Sub New(avatar As IActor)
        MyBase.New(avatar)
    End Sub

    Public Sub Push(actor As IActorModel) Implements IAvatarStackModel.Push
        avatar.Universe.PushAvatar(ActorModel.GetActor(actor))
    End Sub

    Public Sub Pop() Implements IAvatarStackModel.Pop
        avatar.Universe.PopAvatar()
    End Sub

    Friend Shared Function FromActor(avatar As IActor) As IAvatarStackModel
        Return New AvatarStackModel(avatar)
    End Function
End Class

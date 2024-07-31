Imports FHOS.Persistence

Friend Class AvatarStackModel
    Inherits BaseAvatarModel
    Implements IAvatarStackModel

    Protected Sub New(actor As IActor)
        MyBase.New(actor)
    End Sub

    Public Sub Push(actor As IActorModel) Implements IAvatarStackModel.Push
        MyBase.actor.Universe.Avatar.SetActor(ActorModel.GetActor(actor))
    End Sub

    Public Sub Pop() Implements IAvatarStackModel.Pop
        actor.Universe.Avatar.RemoveActor()
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarStackModel
        Return New AvatarStackModel(actor)
    End Function
End Class

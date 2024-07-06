Imports FHOS.Persistence

Friend MustInherit Class AvatarYokedModel
    Inherits BaseAvatarModel
    Implements IAvatarYokedModel

    Private ReadOnly yokeType As String

    Sub New(actor As IActor, yokeType As String)
        MyBase.New(actor)
        Me.yokeType = yokeType
    End Sub

    Public ReadOnly Property IsActive As Boolean Implements IAvatarYokedModel.IsActive
        Get
            Return Specimen IsNot Nothing
        End Get
    End Property

    Protected ReadOnly Property YokedActor As IActor
        Get
            Return actor.Yokes.Actor(yokeType)
        End Get
    End Property

    Public ReadOnly Property Specimen As IActorModel Implements IAvatarYokedModel.Specimen
        Get
            Return ActorModel.FromActor(YokedActor)
        End Get
    End Property

    Public Sub Leave() Implements IAvatarYokedModel.Leave
        OnLeave()
        actor.Yokes.Actor(yokeType) = Nothing
    End Sub

    Protected MustOverride Sub OnLeave()
End Class

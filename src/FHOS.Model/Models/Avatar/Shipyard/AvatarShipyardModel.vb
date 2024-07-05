Imports FHOS.Persistence

Friend Class AvatarShipyardModel
    Implements IAvatarShipyardModel

    Private actor As IActor

    Public Sub New(actor As IActor)
        Me.actor = actor
    End Sub
    Private ReadOnly Property YokedShipyard As IActor
        Get
            Return actor.Yokes.Actor(YokeTypes.Shipyard)
        End Get
    End Property

    Public ReadOnly Property IsActive As Boolean Implements IAvatarShipyardModel.IsActive
        Get
            Return YokedShipyard IsNot Nothing
        End Get
    End Property

    Public ReadOnly Property Shipyard As IActorModel Implements IAvatarShipyardModel.Shipyard
        Get
            Return ActorModel.FromActor(YokedShipyard)
        End Get
    End Property

    Public Sub Leave() Implements IAvatarShipyardModel.Leave
        actor.Yokes.Actor(YokeTypes.Shipyard) = Nothing
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarShipyardModel
        Return New AvatarShipyardModel(actor)
    End Function
End Class

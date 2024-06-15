Imports FHOS.Persistence

Friend Class AvatarTraderModel
    Implements IAvatarTraderModel

    Private ReadOnly actor As IActor

    Public Sub New(actor As IActor)
        Me.actor = actor
    End Sub

    Public ReadOnly Property IsActive As Boolean Implements IAvatarTraderModel.IsActive
        Get
            Return actor.Yokes.Actor(YokeTypes.Trader) IsNot Nothing
        End Get
    End Property

    Public Sub Leave() Implements IAvatarTraderModel.Leave
        actor.Yokes.Actor(YokeTypes.Trader) = Nothing
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarTraderModel
        Return New AvatarTraderModel(actor)
    End Function
End Class

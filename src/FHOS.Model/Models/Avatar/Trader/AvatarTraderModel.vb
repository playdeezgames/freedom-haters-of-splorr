Imports FHOS.Persistence

Friend Class AvatarTraderModel
    Implements IAvatarTraderModel

    Private ReadOnly actor As IActor

    Public Sub New(actor As IActor)
        Me.actor = actor
    End Sub

    Public ReadOnly Property IsActive As Boolean Implements IAvatarTraderModel.IsActive
        Get
            Return actor.Yokes.Actor(YokeTypes.StarGate) IsNot Nothing
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarTraderModel
        Return New AvatarTraderModel(actor)
    End Function
End Class

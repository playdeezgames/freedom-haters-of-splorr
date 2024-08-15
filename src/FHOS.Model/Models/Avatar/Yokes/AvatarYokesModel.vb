Imports FHOS.Persistence

Friend Class AvatarYokesModel
    Implements IAvatarYokesModel

    Private ReadOnly actor As IActor

    Public Sub New(actor As IActor)
        Me.actor = actor
    End Sub

    Public ReadOnly Property Interaction As IAvatarInteractionModel Implements IAvatarYokesModel.Interaction
        Get
            Return AvatarInteractionModel.FromActor(actor)
        End Get
    End Property

    Public ReadOnly Property Trader As IAvatarTraderModel Implements IAvatarYokesModel.Trader
        Get
            Return AvatarTraderModel.FromActor(actor)
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarYokesModel
        Return New AvatarYokesModel(actor)
    End Function
End Class

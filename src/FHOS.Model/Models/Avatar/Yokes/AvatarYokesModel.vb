Imports FHOS.Persistence

Friend Class AvatarYokesModel
    Implements IAvatarYokesModel

    Private ReadOnly actor As IActor

    Public Sub New(actor As IActor)
        Me.actor = actor
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarYokesModel
        Return New AvatarYokesModel(actor)
    End Function
End Class

Imports FHOS.Persistence

Friend Class AvatarInventoryModel
    Implements IAvatarInventoryModel

    Private ReadOnly actor As IActor

    Public Sub New(actor As IActor)
        Me.actor = actor
    End Sub

    Public ReadOnly Property Summary As IEnumerable(Of (Text As String, Item As String)) Implements IAvatarInventoryModel.Summary
        Get
            Return Array.Empty(Of (Text As String, Item As String))
        End Get
    End Property

    Friend Shared Function FromActor(actor As IActor) As IAvatarInventoryModel
        Return New AvatarInventoryModel(actor)
    End Function
End Class

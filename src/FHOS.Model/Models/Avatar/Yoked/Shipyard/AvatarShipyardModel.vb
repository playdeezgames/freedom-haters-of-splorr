Imports FHOS.Persistence

Friend Class AvatarShipyardModel
    Inherits AvatarYokedModel
    Implements IAvatarShipyardModel

    Public Sub New(actor As IActor)
        MyBase.New(actor, YokeTypes.Shipyard)
    End Sub

    Protected Overrides Sub OnLeave()
        'do nothing
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarShipyardModel
        Return New AvatarShipyardModel(actor)
    End Function
End Class

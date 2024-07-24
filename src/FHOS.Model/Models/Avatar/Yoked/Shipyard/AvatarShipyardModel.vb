Imports FHOS.Persistence

Friend Class AvatarShipyardModel
    Inherits AvatarYokedModel
    Implements IAvatarShipyardModel

    Public Sub New(actor As IActor)
        MyBase.New(actor, YokeTypes.Shipyard)
    End Sub

    Public ReadOnly Property CanChangeEquipment As Boolean Implements IAvatarShipyardModel.CanChangeEquipment
        Get
            'determine equipment occupying required equipment slots
            'TODO: detect equipment that is not optional for which i have a replacement in inventory
            Throw New NotImplementedException()
        End Get
    End Property

    Protected Overrides Sub OnLeave()
        'do nothing
    End Sub

    Friend Shared Function FromActor(actor As IActor) As IAvatarShipyardModel
        Return New AvatarShipyardModel(actor)
    End Function
End Class

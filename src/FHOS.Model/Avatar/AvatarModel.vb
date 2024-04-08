Friend Class AvatarModel
    Implements IAvatarModel

    Private ReadOnly world As Persistence.IWorld

    Public Sub New(world As Persistence.IWorld)
        Me.world = world
    End Sub

    Public ReadOnly Property X As Integer Implements IAvatarModel.X
        Get
            Return world.Avatar.Cell.Column
        End Get
    End Property

    Public ReadOnly Property Y As Integer Implements IAvatarModel.Y
        Get
            Return world.Avatar.Cell.Row
        End Get
    End Property

    Public ReadOnly Property Facing As Integer Implements IAvatarModel.Facing
        Get
            Return world.Avatar.Facing
        End Get
    End Property
End Class

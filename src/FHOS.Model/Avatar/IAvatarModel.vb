Public Interface IAvatarModel
    ReadOnly Property X As Integer
    ReadOnly Property Y As Integer
    ReadOnly Property Facing As Integer
    Sub Move(delta As (X As Integer, Y As Integer))
End Interface

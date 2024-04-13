Public Interface IAvatarModel
    ReadOnly Property X As Integer
    ReadOnly Property Y As Integer
    Sub Move(delta As (X As Integer, Y As Integer))
    Sub SetFacing(facing As Integer)
    ReadOnly Property MapName As String
    ReadOnly Property OxygenPercent As Integer
    ReadOnly Property OxygenHue As Integer
    ReadOnly Property HasActions As Boolean
    ReadOnly Property StarSystem As IStarSystemModel
End Interface

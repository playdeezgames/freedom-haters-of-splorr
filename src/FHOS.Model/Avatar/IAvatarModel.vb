Public Interface IAvatarModel
    ReadOnly Property X As Integer
    ReadOnly Property Y As Integer
    ReadOnly Property MapName As String
    ReadOnly Property HasActions As Boolean

    Sub Move(delta As (X As Integer, Y As Integer))
    Sub SetFacing(facing As Integer)

    ReadOnly Property OxygenPercent As Integer
    ReadOnly Property OxygenHue As Integer

    ReadOnly Property Tutorial As IAvatarTutorialModel
    ReadOnly Property StarSystem As IAvatarStarSystemModel
End Interface

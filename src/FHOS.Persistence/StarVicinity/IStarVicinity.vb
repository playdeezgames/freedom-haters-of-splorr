Public Interface IStarVicinity
    ReadOnly Property Id As Integer
    ReadOnly Property Name As String
    ReadOnly Property Universe As IUniverse
    ReadOnly Property StarType As String
    Property Map As IMap
    Function CreateStar() As IStar
End Interface

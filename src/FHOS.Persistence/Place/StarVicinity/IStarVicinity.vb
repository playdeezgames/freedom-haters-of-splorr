Public Interface IStarVicinity
    Inherits IStar
    ReadOnly Property StarType As String
    Function CreateStar() As IStar
End Interface

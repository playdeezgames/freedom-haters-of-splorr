Public Interface IStarVicinity
    Inherits IPlace
    ReadOnly Property StarType As String
    Function CreateStar() As IStar
End Interface

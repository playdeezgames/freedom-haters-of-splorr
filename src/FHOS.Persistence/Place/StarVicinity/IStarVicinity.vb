Public Interface IStarVicinity
    Inherits IPlace
    ReadOnly Property StarType As String
    Function CreateStar() As IPlace
End Interface

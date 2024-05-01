Public Interface IStarVicinity
    Inherits IPlace
    ReadOnly Property StarType As String
    Function LegacyCreateStar() As IPlace
    Function CreateStar() As IStar
End Interface

Public Interface IPlaceFactory
    Function CreateStarVicinity(
                               x As Integer,
                               y As Integer) As IPlace
    Function CreateStar() As IPlace
End Interface

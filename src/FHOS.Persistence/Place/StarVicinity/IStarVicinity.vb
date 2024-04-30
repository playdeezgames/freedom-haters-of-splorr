Public Interface IStarVicinity
    Inherits IPlace
    ReadOnly Property Name As String
    ReadOnly Property Universe As IUniverse
    ReadOnly Property StarType As String
    Property Map As IMap
    Function CreateStar() As IPlace
    ReadOnly Property Identifier As String
End Interface

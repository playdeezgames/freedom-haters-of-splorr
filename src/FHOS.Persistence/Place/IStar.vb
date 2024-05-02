Public Interface IPlace
    ReadOnly Property Id As Integer
    ReadOnly Property Universe As IUniverse
    Property Name As String
    Property Map As IMap
    ReadOnly Property Identifier As String
    ReadOnly Property PlaceType As String
    ReadOnly Property Parent As IPlace
End Interface

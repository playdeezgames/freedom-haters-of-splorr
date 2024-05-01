Public Interface IPlace
    ReadOnly Property Id As Integer
    ReadOnly Property Universe As IUniverse
    ReadOnly Property Name As String
    Property Map As IMap
    ReadOnly Property Identifier As String
End Interface

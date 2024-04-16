Public Interface IStarSystem
    ReadOnly Property Id As Integer
    ReadOnly Property Name As String
    Property Map As IMap
    ReadOnly Property Universe As IUniverse
End Interface

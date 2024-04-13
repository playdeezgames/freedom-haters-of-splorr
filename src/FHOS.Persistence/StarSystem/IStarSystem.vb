Public Interface IStarSystem
    ReadOnly Property Id As Integer
    ReadOnly Property Name As String
    Property Map As IMap
    ReadOnly Property World As IWorld
End Interface

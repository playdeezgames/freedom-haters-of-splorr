Public Interface IMap
    Function GetCell(column As Integer, row As Integer) As ICell
    ReadOnly Property World As IWorld
End Interface

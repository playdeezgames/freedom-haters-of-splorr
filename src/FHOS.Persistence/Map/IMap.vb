Public Interface IMap
    Function GetCell(column As Integer, row As Integer) As ICell
    ReadOnly Property Cells As IEnumerable(Of ICell)
    ReadOnly Property World As IUniverse
    ReadOnly Property Name As String
    ReadOnly Property Id As Integer
    ReadOnly Property Size As (Columns As Integer, Rows As Integer)
End Interface

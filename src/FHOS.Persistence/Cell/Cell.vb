Friend Class Cell
    Inherits CellDataClient
    Implements ICell

    Public Sub New(worldData As Data.WorldData, cellId As Integer)
        MyBase.New(worldData, cellId)
    End Sub

    Public ReadOnly Property Id As Integer Implements ICell.Id
        Get
            Return CellId
        End Get
    End Property
End Class

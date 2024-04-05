Imports FHOS.Data

Friend Class CellDataClient
    Inherits WorldDataClient

    Public Sub New(worldData As WorldData, cellId As Integer)
        MyBase.New(worldData)
        Me.CellId = cellId
    End Sub

    Protected ReadOnly Property CellId As Integer
    Protected ReadOnly Property CellData As CellData
        Get
            Return WorldData.Cells(CellId)
        End Get
    End Property
End Class

Imports FHOS.Data

Friend Class CellDataClient
    Inherits WorldDataClient

    Public Sub New(worldData As UniverseData, cellId As Integer)
        MyBase.New(worldData)
        Me.CellId = cellId
    End Sub

    Protected ReadOnly Property CellId As Integer
    Protected ReadOnly Property CellData As LocationData
        Get
            Return WorldData.Locations(CellId)
        End Get
    End Property
End Class

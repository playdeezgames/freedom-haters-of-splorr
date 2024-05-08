Imports FHOS.Data

Friend Class MapDataClient
    Inherits EntityDataClient
    Protected ReadOnly Property MapData As MapData
        Get
            Return UniverseData.Maps.Entities(Id)
        End Get
    End Property
    Sub New(worldData As UniverseData, mapId As Integer)
        MyBase.New(worldData, mapId)
    End Sub
End Class

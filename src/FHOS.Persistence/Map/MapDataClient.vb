Imports FHOS.Data

Friend Class MapDataClient
    Inherits UniverseDataClient
    Protected ReadOnly MapId As Integer
    Protected ReadOnly Property MapData As MapData
        Get
            Return UniverseData.Maps.Entities(MapId)
        End Get
    End Property
    Sub New(worldData As UniverseData, mapId As Integer)
        MyBase.New(worldData)
        Me.MapId = mapId
    End Sub
End Class

Imports FHOS.Data

Friend Class MapDataClient
    Inherits WorldDataClient
    Protected ReadOnly MapId As Integer
    Protected ReadOnly Property MapData As MapData
        Get
            Return WorldData.LegacyMaps(MapId)
        End Get
    End Property
    Sub New(worldData As UniverseData, mapId As Integer)
        MyBase.New(worldData)
        Me.MapId = mapId
    End Sub
End Class

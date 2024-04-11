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

    Public ReadOnly Property Map As IMap Implements ICell.Map
        Get
            Return New Map(WorldData, CellData.Statistics(StatisticTypes.MapId))
        End Get
    End Property

    Public ReadOnly Property Column As Integer Implements ICell.Column
        Get
            Return CellData.Statistics(StatisticTypes.Column)
        End Get
    End Property

    Public ReadOnly Property Row As Integer Implements ICell.Row
        Get
            Return CellData.Statistics(StatisticTypes.Row)
        End Get
    End Property

    Public Property TerrainType As String Implements ICell.TerrainType
        Get
            Return CellData.Metadatas(MetadataTypes.TerrainType)
        End Get
        Set(value As String)
            CellData.Metadatas(MetadataTypes.TerrainType) = value
        End Set
    End Property

    Public Property Character As ICharacter Implements ICell.Character
        Get
            Dim characterId As Integer
            If CellData.Statistics.TryGetValue(StatisticTypes.CharacterId, characterId) Then
                Return New Character(WorldData, characterId)
            End If
            Return Nothing
        End Get
        Set(value As ICharacter)
            If value Is Nothing Then
                CellData.Statistics.Remove(StatisticTypes.CharacterId)
                Return
            End If
            CellData.Statistics(StatisticTypes.CharacterId) = value.Id
        End Set
    End Property

    Public Property StarSystem As IStarSystem Implements ICell.StarSystem
        Get
            Dim starSystemId As Integer
            If CellData.Statistics.TryGetValue(StatisticTypes.StarSystemId, starSystemId) Then
                Return New StarSystem(WorldData, starSystemId)
            End If
            Return Nothing
        End Get
        Set(value As IStarSystem)
            If value IsNot Nothing Then
                CellData.Statistics(StatisticTypes.StarSystemId) = value.Id
            Else
                CellData.Statistics.Remove(StatisticTypes.StarSystemId)
            End If
        End Set
    End Property
End Class

Friend Class Cell
    Inherits CellDataClient
    Implements ICell

    Public Sub New(worldData As Data.UniverseData, cellId As Integer)
        MyBase.New(worldData, cellId)
    End Sub

    Public ReadOnly Property Id As Integer Implements ICell.Id
        Get
            Return CellId
        End Get
    End Property

    Public ReadOnly Property Map As IMap Implements ICell.Map
        Get
            Return New Map(UniverseData, CellData.Statistics(StatisticTypes.MapId))
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
                Return New Character(UniverseData, characterId)
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
                Return New StarSystem(UniverseData, starSystemId)
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

    Public Property Teleporter As ITeleporter Implements ICell.Teleporter
        Get
            Dim teleporterId As Integer
            If CellData.Statistics.TryGetValue(StatisticTypes.TeleporterId, teleporterId) Then
                Return New Teleporter(UniverseData, teleporterId)
            End If
            Return Nothing
        End Get
        Set(value As ITeleporter)
            If value IsNot Nothing Then
                CellData.Statistics(StatisticTypes.TeleporterId) = value.Id
            Else
                CellData.Statistics.Remove(StatisticTypes.TeleporterId)
            End If
        End Set
    End Property

    Public Property Tutorial As String Implements ICell.Tutorial
        Get
            Dim result As String = Nothing
            If CellData.Metadatas.TryGetValue(MetadataTypes.Tutorial, result) Then
                Return result
            End If
            Return Nothing
        End Get
        Set(value As String)
            If value Is Nothing Then
                CellData.Metadatas.Remove(MetadataTypes.Tutorial)
            Else
                CellData.Metadatas(MetadataTypes.Tutorial) = value
            End If
        End Set
    End Property

    Public ReadOnly Property HasTeleporter As Boolean Implements ICell.HasTeleporter
        Get
            Return CellData.Statistics.ContainsKey(StatisticTypes.TeleporterId)
        End Get
    End Property

    Public Sub SetFlag(flag As String) Implements ICell.SetFlag
        CellData.Flags.Add(flag)
    End Sub

    Public Function HasFlag(name As String) As Boolean Implements ICell.HasFlag
        Return CellData.Flags.Contains(name)
    End Function
End Class

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
            Return CellData.Row
        End Get
    End Property

    Public Property TerrainType As String Implements ICell.TerrainType
        Get
            Return CellData.TerrainType
        End Get
        Set(value As String)
            CellData.TerrainType = value
        End Set
    End Property

    Public Property Character As ICharacter Implements ICell.Character
        Get
            If CellData.CharacterId.HasValue Then
                Return New Character(WorldData, CellData.CharacterId.Value)
            End If
            Return Nothing
        End Get
        Set(value As ICharacter)
            If value Is Nothing Then
                CellData.CharacterId = Nothing
                Return
            End If
            CellData.CharacterId = value.Id
        End Set
    End Property

    Public Property StarSystem As IStarSystem Implements ICell.StarSystem
        Get
            If CellData.StarSystemId.HasValue Then
                Return New StarSystem(WorldData, CellData.StarSystemId.Value)
            End If
            Return Nothing
        End Get
        Set(value As IStarSystem)
            If value IsNot Nothing Then
                CellData.StarSystemId = value.Id
            Else
                CellData.StarSystemId = Nothing
            End If
        End Set
    End Property
End Class

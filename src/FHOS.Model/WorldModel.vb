Imports System.IO
Imports System.Text.Json
Imports FHOS.Data
Imports FHOS.Persistence

Public Class WorldModel
    Implements IWorldModel
    Const BoardRadius = 6

    Private worldData As WorldData = Nothing

    Public ReadOnly Property Board As IReadOnlyDictionary(Of Integer, IReadOnlyDictionary(Of Integer, (TerrainType As String, Character As (CharacterType As String, Facing As Integer)))) Implements IWorldModel.Board
        Get
            Return Enumerable.
                Range(-BoardRadius, BoardRadius * 2 + 1).
                ToDictionary(
                    Function(x As Integer) x,
                    Function(boardColumn As Integer) As IReadOnlyDictionary(Of Integer, (TerrainType As String, Character As (CharacterType As String, Facing As Integer)))
                        Return Enumerable.
                            Range(-BoardRadius, BoardRadius * 2 + 1).
                            ToDictionary(
                                Function(y As Integer) y,
                                Function(boardRow As Integer) As (TerrainType As String, Character As (CharacterType As String, Facing As Integer))
                                    Return GetBoardCell(boardColumn, boardRow)
                                End Function)
                    End Function)
        End Get
    End Property

    Private Function GetBoardCell(boardColumn As Integer, boardRow As Integer) As (TerrainType As String, Character As (CharacterType As String, Facing As Integer))
        Dim avatar As ICharacter = World.Avatar
        Dim avatarCell As ICell = avatar.Cell
        Dim map As IMap = avatarCell.Map
        Dim mapColumn = boardColumn + avatarCell.Column
        Dim mapRow = boardRow + avatarCell.Row
        Dim cell = map.GetCell(mapColumn, mapRow)
        Return (cell?.TerrainType, (cell?.Character?.CharacterType, If(cell?.Character?.Facing, 0)))
    End Function

    Private ReadOnly Property World As IWorld
        Get
            Return New World(worldData)
        End Get
    End Property

    Public Sub Save(filename As String) Implements IWorldModel.Save
        File.WriteAllText(filename, JsonSerializer.Serialize(worldData))
    End Sub

    Public Sub Load(filename As String) Implements IWorldModel.Load
        worldData = JsonSerializer.Deserialize(Of WorldData)(File.ReadAllText(filename))
    End Sub

    Public Sub Abandon() Implements IWorldModel.Abandon
        worldData = Nothing
    End Sub

    Public Sub Embark() Implements IWorldModel.Embark
        worldData = New WorldData()
        WorldInitializer.Initialize(World)
    End Sub
End Class

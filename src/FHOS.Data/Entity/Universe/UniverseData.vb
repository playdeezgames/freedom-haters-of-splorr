Imports System.Text.Json.Serialization

Public Class UniverseData
    Inherits EntityData
    Implements IUniverseData
    Public Sub New(
                  connection As SqliteConnection)
        MyBase.New(connection)
        CreateFlagsTable()
        CreateStatisticsTable()
        CreateMetadatasTable()
    End Sub

    Const StatisticTableName = "UniverseStatistics"
    Const StatisticTypeColumn = "StatisticType"
    Const StatisticValueColumn = "StatisticValue"

    Const MetadataTableName = "UniverseMetadatas"
    Const MetadataTypeColumn = "MetadataType"
    Const MetadataValueColumn = "MetadataValue"

    Private Sub CreateStatisticsTable()
        Using command = _connection.CreateCommand()
            command.CommandText = $"CREATE TABLE IF NOT EXISTS [{StatisticTableName}]([{StatisticTypeColumn}] TEXT NOT NULL UNIQUE,[{StatisticValueColumn}] INT NOT NULL);"
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub CreateMetadatasTable()
        Using command = _connection.CreateCommand()
            command.CommandText = $"
CREATE TABLE IF NOT EXISTS [{MetadataTableName}]
(
    [{MetadataTypeColumn}] TEXT NOT NULL UNIQUE,
    [{MetadataValueColumn}] TEXT NOT NULL
);"
            command.ExecuteNonQuery()
        End Using
    End Sub

    Public Property Actors As New Dictionary(Of Integer, IActorData) Implements IUniverseData.Actors
    Property Locations As New Dictionary(Of Integer, ILocationData) Implements IUniverseData.Locations
    Property Maps As New Dictionary(Of Integer, IMapData) Implements IUniverseData.Maps
    Property Groups As New Dictionary(Of Integer, IGroupData) Implements IUniverseData.Groups
    Property Stores As New Dictionary(Of Integer, IStoreData) Implements IUniverseData.Stores
    Property Items As New Dictionary(Of Integer, IItemData) Implements IUniverseData.Items
    Property Avatars As New Stack(Of Integer) Implements IUniverseData.Avatars
    <JsonIgnore>
    Property Messages As New Queue(Of MessageData) Implements IUniverseData.Messages
    Private _nextActorId As Integer = 0
    Private _nextLocationId As Integer = 0
    Private _nextMapId As Integer = 0
    Private _nextGroupId As Integer = 0
    Private _nextStoreId As Integer = 0
    Private _nextItemId As Integer = 0
    Public ReadOnly Property NextActorId As Integer Implements IUniverseData.NextActorId
        Get
            Dim actorId = _nextActorId
            _nextActorId += 1
            Return actorId
        End Get
    End Property

    Public ReadOnly Property NextLocationId As Integer Implements IUniverseData.NextLocationId
        Get
            Dim locationId = _nextLocationId
            _nextLocationId += 1
            Return locationId
        End Get
    End Property

    Public ReadOnly Property NextMapId As Integer Implements IUniverseData.NextMapId
        Get
            Dim mapId = _nextMapId
            _nextMapId += 1
            Return mapId
        End Get
    End Property

    Public ReadOnly Property NextGroupId As Integer Implements IUniverseData.NextGroupId
        Get
            Dim groupId = _nextGroupId
            _nextGroupId += 1
            Return groupId
        End Get
    End Property

    Public ReadOnly Property NextStoreId As Integer Implements IUniverseData.NextStoreId
        Get
            Dim storeId = _nextStoreId
            _nextStoreId += 1
            Return storeId
        End Get
    End Property

    Public ReadOnly Property NextItemId As Integer Implements IUniverseData.NextItemId
        Get
            Dim itemId = _nextItemId
            _nextItemId += 1
            Return itemId
        End Get
    End Property

    Public ReadOnly Property Connection As SqliteConnection Implements IUniverseData.Connection
        Get
            Return _connection
        End Get
    End Property

    Public Function GetActorData(actorId As Integer) As IActorData Implements IUniverseData.GetActorData
        Dim actorData As IActorData = Nothing
        If Actors.TryGetValue(actorId, actorData) Then
            Return actorData
        End If
        Return Nothing
    End Function

    Public Function GetLocationData(locationId As Integer) As ILocationData Implements IUniverseData.GetLocationData
        Return Nothing
    End Function

    Public Function GetMapData(mapId As Integer) As IMapData Implements IUniverseData.GetMapData
        Return Nothing
    End Function

    Public Function GetGroupData(groupId As Integer) As IGroupData Implements IUniverseData.GetGroupData
        Return Nothing
    End Function

    Public Function GetStoreData(storeId As Integer) As IStoreData Implements IUniverseData.GetStoreData
        Return Nothing
    End Function

    Public Function GetItemData(storeId As Integer) As IItemData Implements IUniverseData.GetItemData
        Return Nothing
    End Function

    Protected Overrides Sub WriteDatabaseFlag(flagType As String)
        Using command = _connection.CreateCommand
            command.CommandText = $"INSERT OR IGNORE INTO [{FlagTableName}]([{FlagTypeColumn}]) VALUES(@{FlagTypeColumn});"
            command.Parameters.AddWithValue(FlagTypeColumn, flagType)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Const FlagTableName = "UniverseFlags"
    Private Const FlagTypeColumn = "FlagType"

    Private Sub CreateFlagsTable()
        Using command = _connection.CreateCommand()
            command.CommandText = $"CREATE TABLE IF NOT EXISTS [{FlagTableName}]([{FlagTypeColumn}] TEXT NOT NULL UNIQUE);"
            command.ExecuteNonQuery()
        End Using
    End Sub

    Protected Overrides Sub ClearDatabaseFlag(flagType As String)
        Using command = _connection.CreateCommand
            command.CommandText = $"DELETE FROM [{FlagTableName}] WHERE [{FlagTypeColumn}]=@{FlagTypeColumn};"
            command.Parameters.AddWithValue(FlagTypeColumn, flagType)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Protected Overrides Function ReadDatabaseFlag(flagType As String) As Boolean
        Using command = _connection.CreateCommand
            command.CommandText = $"SELECT COUNT(1) FROM [{FlagTableName}] WHERE [{FlagTypeColumn}]=@{FlagTypeColumn};"
            command.Parameters.AddWithValue(FlagTypeColumn, flagType)
            Return CInt(command.ExecuteScalar) > 0
        End Using
    End Function

    Protected Overrides Sub WriteDatabaseStatistic(statisticType As String, statisticValue As Integer)
        Using command = _connection.CreateCommand
            command.CommandText = $"
INSERT INTO [{StatisticTableName}]
(
    [{StatisticTypeColumn}],
    [{StatisticValueColumn}]
) 
VALUES
(
    @{StatisticTypeColumn},
    @{StatisticValueColumn}
)
ON 
    CONFLICT([{StatisticTypeColumn}]) 
DO 
    UPDATE 
    SET 
        [{StatisticValueColumn}]=@{StatisticValueColumn} 
    WHERE 
        [{StatisticTypeColumn}]=@{StatisticTypeColumn};"
            command.Parameters.AddWithValue(StatisticTypeColumn, statisticType)
            command.Parameters.AddWithValue(StatisticValueColumn, statisticValue)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Protected Overrides Sub ClearDatabaseStatistic(statisticType As String)
        Using command = _connection.CreateCommand
            command.CommandText = $"
DELETE FROM 
    [{StatisticTableName}] 
WHERE 
    [{StatisticTypeColumn}]=@{StatisticTypeColumn};"
            command.Parameters.AddWithValue(StatisticTypeColumn, statisticType)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Protected Overrides Function ReadDatabaseStatistic(statisticType As String) As Integer?
        Using command = _connection.CreateCommand
            command.CommandText = $"
SELECT 
    [{StatisticValueColumn}] 
FROM 
    [{StatisticTableName}] 
WHERE 
    [{StatisticTypeColumn}]=@{StatisticTypeColumn};"
            command.Parameters.AddWithValue(StatisticTypeColumn, statisticType)
            Using reader = command.ExecuteReader
                If reader.Read Then
                    Return reader.GetInt32(0)
                End If
            End Using
            Return Nothing
        End Using
    End Function

    Protected Overrides Sub WriteDatabaseMetadata(metadataType As String, metadataValue As String)
        Using command = _connection.CreateCommand
            command.CommandText = $"
INSERT INTO [{MetadataTableName}]
(
    [{MetadataTypeColumn}],
    [{MetadataValueColumn}]
) 
VALUES
(
    @{MetadataTypeColumn},
    @{MetadataValueColumn}
)
ON 
    CONFLICT([{MetadataTypeColumn}]) 
DO 
    UPDATE 
    SET 
        [{MetadataValueColumn}]=@{MetadataValueColumn} 
    WHERE 
        [{MetadataTypeColumn}]=@{MetadataTypeColumn};"
            command.Parameters.AddWithValue(MetadataTypeColumn, metadataType)
            command.Parameters.AddWithValue(MetadataValueColumn, metadataValue)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Protected Overrides Sub ClearDatabaseMetadata(metadataType As String)
        Using command = _connection.CreateCommand
            command.CommandText = $"
DELETE FROM 
    [{MetadataTableName}] 
WHERE 
    [{MetadataTypeColumn}]=@{MetadataTypeColumn};"
            command.Parameters.AddWithValue(MetadataTypeColumn, metadataType)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Protected Overrides Function ReadDatabaseMetadata(metadataType As String) As String
        Using command = _connection.CreateCommand
            command.CommandText = $"
SELECT 
    [{MetadataValueColumn}] 
FROM 
    [{MetadataTableName}] 
WHERE 
    [{MetadataTypeColumn}]=@{MetadataTypeColumn};"
            command.Parameters.AddWithValue(MetadataTypeColumn, metadataType)
            Using reader = command.ExecuteReader
                If reader.Read Then
                    Return reader.GetString(0)
                End If
            End Using
            Return Nothing
        End Using
    End Function
End Class

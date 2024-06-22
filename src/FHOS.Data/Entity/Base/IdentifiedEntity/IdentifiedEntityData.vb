Public MustInherit Class IdentifiedEntityData
    Inherits EntityData
    Implements IIdentifiedEntityData
    Protected ReadOnly tablePrefix As String
    Public Sub New(
                  connection As SqliteConnection,
                  tablePrefix As String,
                  id As Integer,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        MyBase.New(connection, metadatas:=metadatas)
        Me.Id = id
        Me.tablePrefix = tablePrefix
        CreateFlagsTable()
        CreateStatisticsTable()
    End Sub

    Const StatisticTableSuffix = "Statistics"
    Const StatisticTypeColumn = "StatisticType"
    Const StatisticValueColumn = "StatisticValue"

    Private Sub CreateStatisticsTable()
        Using command = _connection.CreateCommand()
            command.CommandText = $"
CREATE TABLE IF NOT EXISTS [{tablePrefix}{StatisticTableSuffix}]
(
    [{tablePrefix}Id] INTEGER NOT NULL, 
    [{StatisticTypeColumn}] TEXT NOT NULL, 
    [{StatisticValueColumn}] INT NOT NULL, 
    UNIQUE([{tablePrefix}Id],[{StatisticTypeColumn}])
);"
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Const FlagTableSuffix = "Flags"
    Private Const FlagTypeColumn = "FlagType"

    Private Sub CreateFlagsTable()
        Using command = _connection.CreateCommand()
            command.CommandText = $"CREATE TABLE IF NOT EXISTS [{tablePrefix}{FlagTableSuffix}]([{tablePrefix}Id] INTEGER NOT NULL, [{FlagTypeColumn}] TEXT NOT NULL, UNIQUE([{tablePrefix}Id],[{FlagTypeColumn}]));"
            command.ExecuteNonQuery()
        End Using
    End Sub

    Public ReadOnly Property Id As Integer Implements IIdentifiedEntityData.Id
    Protected Overrides Sub ClearDatabaseFlag(flagType As String)
        Using command = _connection.CreateCommand
            command.CommandText = $"DELETE FROM [{tablePrefix}{FlagTableSuffix}] WHERE [{FlagTypeColumn}]=@{FlagTypeColumn} AND [{tablePrefix}Id]=@{tablePrefix}Id;"
            command.Parameters.AddWithValue(FlagTypeColumn, flagType)
            command.Parameters.AddWithValue($"{tablePrefix}Id", Id)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Protected Overrides Sub WriteDatabaseFlag(flagType As String)
        Using command = _connection.CreateCommand
            command.CommandText = $"INSERT OR IGNORE INTO [{tablePrefix}{FlagTableSuffix}]([{tablePrefix}Id],[{FlagTypeColumn}]) VALUES(@{tablePrefix}Id,@{FlagTypeColumn});"
            command.Parameters.AddWithValue(FlagTypeColumn, flagType)
            command.Parameters.AddWithValue($"{tablePrefix}Id", Id)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Protected Overrides Function ReadDatabaseFlag(flagType As String) As Boolean
        Using command = _connection.CreateCommand
            command.CommandText = $"SELECT COUNT(1) FROM [{tablePrefix}{FlagTableSuffix}] WHERE [{FlagTypeColumn}]=@{FlagTypeColumn} AND [{tablePrefix}Id]=@{tablePrefix}Id;"
            command.Parameters.AddWithValue(FlagTypeColumn, flagType)
            command.Parameters.AddWithValue($"{tablePrefix}Id", Id)
            Return CInt(command.ExecuteScalar) > 0
        End Using
    End Function
    Protected Overrides Sub WriteDatabaseStatistic(statisticType As String, statisticValue As Integer)
        Using command = _connection.CreateCommand
            command.CommandText = $"
INSERT INTO [{tablePrefix}{StatisticTableSuffix}]
(
    [{tablePrefix}Id],
    [{StatisticTypeColumn}],
    [{StatisticValueColumn}]
) 
VALUES
(
    @{tablePrefix}Id,
    @{StatisticTypeColumn},
    @{StatisticValueColumn}
)
ON 
    CONFLICT([{tablePrefix}Id],[{StatisticTypeColumn}]) 
DO 
    UPDATE 
    SET 
        [{StatisticValueColumn}]=@{StatisticValueColumn} 
    WHERE 
        [{StatisticTypeColumn}]=@{StatisticTypeColumn} AND
        [{tablePrefix}Id]=@{tablePrefix}Id;"
            command.Parameters.AddWithValue($"{tablePrefix}Id", Id)
            command.Parameters.AddWithValue(StatisticTypeColumn, statisticType)
            command.Parameters.AddWithValue(StatisticValueColumn, statisticValue)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Protected Overrides Sub ClearDatabaseStatistic(statisticType As String)
        Using command = _connection.CreateCommand
            command.CommandText = $"
DELETE FROM 
    [{tablePrefix}{StatisticTableSuffix}] 
WHERE 
    [{StatisticTypeColumn}]=@{StatisticTypeColumn} AND
    [{tablePrefix}Id]=@{tablePrefix}Id;"
            command.Parameters.AddWithValue(StatisticTypeColumn, statisticType)
            command.Parameters.AddWithValue($"{tablePrefix}Id", Id)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Protected Overrides Function ReadDatabaseStatistic(statisticType As String) As Integer?
        Using command = _connection.CreateCommand
            command.CommandText = $"
SELECT 
    [{StatisticValueColumn}] 
FROM 
    [{tablePrefix}{StatisticTableSuffix}] 
WHERE 
    [{StatisticTypeColumn}]=@{StatisticTypeColumn} AND
    [{tablePrefix}Id]=@{tablePrefix}Id;"
            command.Parameters.AddWithValue(StatisticTypeColumn, statisticType)
            command.Parameters.AddWithValue($"{tablePrefix}Id", Id)
            Using reader = command.ExecuteReader
                If reader.Read Then
                    Return reader.GetInt32(0)
                End If
            End Using
            Return Nothing
        End Using
    End Function
End Class

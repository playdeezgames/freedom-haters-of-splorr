Public MustInherit Class IdentifiedEntityData
    Inherits EntityData
    Implements IIdentifiedEntityData
    Protected ReadOnly tablePrefix As String
    Public Sub New(
                  connection As SqliteConnection,
                  tablePrefix As String,
                  id As Integer,
                  Optional flags As ISet(Of String) = Nothing,
                  Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        MyBase.New(connection, statistics:=statistics, flags:=flags, metadatas:=metadatas)
        Me.Id = id
        Me.tablePrefix = tablePrefix
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
        CreateFlagsTable()
        Using command = _connection.CreateCommand
            command.CommandText = $"DELETE FROM [{tablePrefix}{FlagTableSuffix}] WHERE [{FlagTypeColumn}]=@{FlagTypeColumn} AND [{tablePrefix}Id]=@{tablePrefix}Id;"
            command.Parameters.AddWithValue(FlagTypeColumn, flagType)
            command.Parameters.AddWithValue($"{tablePrefix}Id", Id)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Protected Overrides Sub SetDatabaseFlag(flagType As String)
        CreateFlagsTable()
        Using command = _connection.CreateCommand
            command.CommandText = $"INSERT OR IGNORE INTO [{tablePrefix}{FlagTableSuffix}]([{tablePrefix}Id],[{FlagTypeColumn}]) VALUES(@{tablePrefix}Id,@{FlagTypeColumn});"
            command.Parameters.AddWithValue(FlagTypeColumn, flagType)
            command.Parameters.AddWithValue($"{tablePrefix}Id", Id)
            command.ExecuteNonQuery()
        End Using
    End Sub
End Class

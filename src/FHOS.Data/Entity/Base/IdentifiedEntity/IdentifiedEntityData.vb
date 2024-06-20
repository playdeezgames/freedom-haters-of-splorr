Public MustInherit Class IdentifiedEntityData
    Inherits EntityData
    Implements IIdentifiedEntityData
    Protected ReadOnly tablePrefix As String
    Public Sub New(
                  connection As SqliteConnection,
                  id As Integer,
                  tablePrefix As String,
                  Optional flags As ISet(Of String) = Nothing,
                  Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        MyBase.New(connection, statistics:=statistics, flags:=flags, metadatas:=metadatas)
        Me.Id = id
        Me.tablePrefix = tablePrefix
        Using command = connection.CreateCommand
            command.CommandText = $"
CREATE TABLE IF NOT EXISTS [{tablePrefix}Flags] 
(
    [{tablePrefix}Id] INTEGER, 
    [FlagType] TEXT,
    UNIQUE ([{tablePrefix}Id],[FlagType]));"
            command.ExecuteNonQuery()
        End Using
    End Sub

    Public ReadOnly Property Id As Integer Implements IIdentifiedEntityData.Id
End Class

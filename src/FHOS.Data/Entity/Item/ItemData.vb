Public Class ItemData
    Inherits IdentifiedEntityData
    Implements IItemData

    Public Sub New(
                  connection As SqliteConnection,
                  id As Integer,
                  Optional flags As ISet(Of String) = Nothing,
                  Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        MyBase.New(connection, id, statistics:=statistics, flags:=flags, metadatas:=metadatas)
    End Sub
End Class

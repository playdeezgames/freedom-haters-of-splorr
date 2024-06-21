Public Class ItemData
    Inherits IdentifiedEntityData
    Implements IItemData

    Public Sub New(
                  connection As SqliteConnection,
                  id As Integer,
                  Optional statistics As IReadOnlyDictionary(Of String, Integer) = Nothing,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        MyBase.New(connection, "Item", id, statistics:=statistics, metadatas:=metadatas)
    End Sub
End Class

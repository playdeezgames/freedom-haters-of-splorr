Public Class ItemData
    Inherits IdentifiedEntityData
    Implements IItemData

    Public Sub New(
                  connection As SqliteConnection,
                  id As Integer,
                  Optional metadatas As IReadOnlyDictionary(Of String, String) = Nothing)
        MyBase.New(connection, "Item", id, metadatas:=metadatas)
    End Sub
End Class
